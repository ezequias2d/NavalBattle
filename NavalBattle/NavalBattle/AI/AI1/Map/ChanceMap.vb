Public Structure ChanceMap
    Private _map As ULong(,)
    Private _houseStatus As HouseStatus()

    Private _width As UInteger
    Private _height As UInteger

    Public Sub New(map As HouseStatus(), width As UInteger, height As UInteger)
        _width = width
        _height = height
        _map = New ULong(width, height) {}
        _houseStatus = map
    End Sub

    Public Sub Adjuster()
        For i As Integer = 0 To _width - 1
            For j As Integer = 0 To _height - 1
                Dim mult As Single = GetHouseMutiplier(i, j)
                _map(i, j) = mult * _map(i, j)
            Next
        Next
    End Sub

    Public Sub AdjusterInvert()
        For i As Integer = 0 To _width - 1
            For j As Integer = 0 To _height - 1
                Dim mult As Single = GetHouseMutiplierInvert(i, j)
                _map(i, j) = mult * _map(i, j)
            Next
        Next
    End Sub

    Public Sub Invert()
        Dim max As ULong = ULong.MinValue
        For Each i As ULong In _map
            max = Math.Max(max, i)
        Next

        For i As Integer = 0 To _width - 1
            For j As Integer = 0 To _height - 1
                If _map(i, j) <> 0 Then
                    _map(i, j) = max - _map(i, j)
                End If
            Next
        Next
    End Sub

    Private Function GetHouseMutiplierInvert(x As Integer, y As Integer) As Single
        If _houseStatus(x + y * Width) = HouseStatus.Hit Then
            Return 0
        End If

        Dim output As Single = 0

        Dim countHorizontal As Integer = 0
        Dim countVertical As Integer = 0

        If x > 0 AndAlso _houseStatus((x - 1) + y * Width) = HouseStatus.Hit Then
            countHorizontal += 1
        End If

        If x < Width - 1 AndAlso _houseStatus((x + 1) + y * Width) = HouseStatus.Hit Then
            countHorizontal += 1
        End If

        If y > 0 AndAlso _houseStatus(x + (y - 1) * Width) = HouseStatus.Hit Then
            countVertical += 1
        End If

        If y < Height - 1 AndAlso _houseStatus(x + (y + 1) * Width) = HouseStatus.Hit Then
            countVertical += 1
        End If

        If countVertical = 0 AndAlso countHorizontal = 0 Then
            output = 1
        Else
            output = 0.1F / (countVertical + countHorizontal)
        End If

        Return output
    End Function

    Private Function GetHouseMutiplier(x As Integer, y As Integer) As Single
        If _houseStatus(x + y * Width) = HouseStatus.Hit Then
            Return 0
        End If

        Dim output As Single = 0

        Dim countHorizontal As Integer = 0
        Dim countVertical As Integer = 0

        If x > 0 AndAlso _houseStatus((x - 1) + y * Width) = HouseStatus.Hit Then
            countHorizontal += 1
        End If

        If x < Width - 1 AndAlso _houseStatus((x + 1) + y * Width) = HouseStatus.Hit Then
            countHorizontal += 1
        End If

        If y > 0 AndAlso _houseStatus(x + (y - 1) * Width) = HouseStatus.Hit Then
            countVertical += 1
        End If

        If y < Height - 1 AndAlso _houseStatus(x + (y + 1) * Width) = HouseStatus.Hit Then
            countVertical += 1
        End If

        If countVertical = 0 AndAlso countHorizontal = 0 Then
            output = 1
        ElseIf countVertical = 0 OrElse countHorizontal = 0 Then
            output = Math.Pow(2, (countHorizontal + countVertical) * 4)
        Else
            output = Math.Pow(2, -(countVertical + countHorizontal) * 3)
        End If

        Return output
    End Function

    Public Sub AddMap(avaliables As (Battleship As UInteger, Carrier As UInteger, Destroyer As UInteger, Submarine As UInteger, Weight As UInteger)())
        Dim orientations As Orientation() = New Orientation() {Orientation.Horizontal, Orientation.Vertical}
        For Each avaliable As (Battleship As UInteger, Carrier As UInteger, Destroyer As UInteger, Submarine As UInteger, Weight As UInteger) In avaliables
            For Each orientation As Orientation In orientations
                For i As UInteger = 0 To _width - 1
                    For j As UInteger = 0 To _height - 1

                        If avaliable.Battleship > 0 Then
                            PutShip(i, j, Ship.Battleship, orientation, avaliable.Weight)
                        End If

                        If avaliable.Carrier > 0 Then
                            PutShip(i, j, Ship.Carrier, orientation, avaliable.Weight)
                        End If

                        If avaliable.Destroyer > 0 Then
                            PutShip(i, j, Ship.Destroyer, orientation, avaliable.Weight)
                        End If

                        If avaliable.Submarine > 0 Then
                            PutShip(i, j, Ship.Submarine, orientation, avaliable.Weight)
                        End If
                    Next

                Next
            Next
        Next
    End Sub

    Public ReadOnly Property Width As UInteger
        Get
            Return _width
        End Get
    End Property

    Public ReadOnly Property Height As UInteger
        Get
            Return _height
        End Get
    End Property

    Public ReadOnly Property Item(x As Integer, y As Integer) As ULong
        Get
            Return _map(x, y)
        End Get
    End Property

    Public Sub CleanMap()
        For i As UInteger = 0 To _width - 1UI
            For j As UInteger = 0 To _height - 1UI
                _map(i, j) = 0UL
            Next
        Next
    End Sub

    Private Sub PutShip(x As Integer, y As Integer, ship As Ship, orientation As Orientation, weight As UInteger)
        If IsPuttable(x, y, ship, orientation) Then
            Dim size As (width As Integer, height As Integer) = GetSize(ship, orientation)

            For i As Integer = x To x + size.width - 1
                For j As Integer = y To y + size.height - 1
                    _map(i, j) = _map(i, j) + weight
                Next
            Next
        End If
    End Sub

    Public Function PutShip(x As Integer, y As Integer, ship As Ship, orientation As Orientation) As Boolean
        If IsPuttable(x, y, ship, orientation) Then
            Dim size As (width As Integer, height As Integer) = GetSize(ship, orientation)

            For i As Integer = x To x + size.width - 1
                For j As Integer = y To y + size.height - 1
                    _houseStatus(i + j * Width) = HouseStatus.Hit
                Next
            Next
            Return True
        End If
        Return False
    End Function

    Private Function IsPuttable(x As Integer, y As Integer, ship As Ship, orientation As Orientation)
        Dim output As Boolean = True
        Dim size As (width As Integer, height As Integer) = GetSize(ship, orientation)

        output = IsFreeArea(x, y, size.width, size.height)

        Return output
    End Function

    Private Function GetSize(ship As Ship, orientation As Orientation) As (width As Integer, height As Integer)
        Dim size As (width As Integer, height As Integer)

        size.width = ship * Convert.ToInt32(orientation = Orientation.Horizontal) + Convert.ToInt32(orientation <> Orientation.Horizontal)
        size.height = ship * Convert.ToInt32(orientation = Orientation.Vertical) + Convert.ToInt32(orientation <> Orientation.Vertical)

        Return size
    End Function

    Private Function IsFreeArea(x As Integer, y As Integer, width As Integer, height As Integer) As Boolean
        Dim output As Boolean = True

        If x + width - 1 < Me.Width AndAlso y + height - 1 < Me.Height Then
            For i As Integer = x To x + width - 1
                For j As Integer = y To y + height - 1
                    output = output AndAlso (_houseStatus(i + j * Me.Width) = HouseStatus.Normal)
                    If Not output Then
                        Exit For
                    End If
                Next
                If Not output Then
                    Exit For
                End If
            Next
        Else
            output = False
        End If

        Return output
    End Function


End Structure
