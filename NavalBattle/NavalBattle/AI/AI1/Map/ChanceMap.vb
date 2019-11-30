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

    Public Function GetMaxHouse() As (x As Integer, y As Integer)
        Dim maxValue As ULong = ULong.MinValue
        Dim output As (x As Integer, y As Integer) = (0, 0)
        For i As Integer = 0 To _width - 1
            For j As Integer = 0 To _height - 1
                If maxValue < _map(i, j) Then
                    maxValue = _map(i, j)
                    output = (i, j)
                End If
            Next
        Next
        Return output
    End Function

    Public Function Min() As ULong
        Dim output As ULong = ULong.MaxValue
        For i As Integer = 0 To _width - 1
            For j As Integer = 0 To _height - 1
                If _map(i, j) <> 0 Then
                    output = Math.Min(_map(i, j), output)
                End If
            Next
        Next
        Return output
    End Function

    Public Function Max() As ULong
        Dim output As ULong = ULong.MinValue
        For i As Integer = 0 To _width - 1
            For j As Integer = 0 To _height - 1
                If _map(i, j) <> 0 Then
                    output = Math.Max(_map(i, j), output)
                End If
            Next
        Next
        Return output
    End Function

    Public Sub Adjuster()
        Dim mini As ULong = Min()
        For i As Integer = 0 To _width - 1
            For j As Integer = 0 To _height - 1
                If _houseStatus(i + j * Width) = HouseStatus.Hit Then
                    _map(i, j) = 0
                Else
                    _map(i, j) = _map(i, j) / mini
                End If
            Next
        Next
    End Sub

    Public Function AVG() As ULong
        Dim med As ULong = 0
        Dim div As ULong = 0
        For i As Integer = 0 To _width - 1
            For j As Integer = 0 To _height - 1
                If _map(i, j) > 0 Then
                    med += _map(i, j)
                    div += 1
                End If
            Next
        Next
        If div <> 0 Then
            Return med / div
        Else
            Return 0
        End If
    End Function

    Public Sub IsolateLargerHouses()
        Dim med As ULong = AVG()

        For i As Integer = 0 To _width - 1
            For j As Integer = 0 To _height - 1
                If _map(i, j) < med Then
                    _map(i, j) = 0
                End If
            Next
        Next
    End Sub

    Public Sub AdjusterInvert()
        For i As Integer = 0 To _width - 1
            For j As Integer = 0 To _height - 1
                Dim mult As Single = GetHouseMutiplierInvert(i, j)
                If Not (_map(i, j) = 1 AndAlso (_map(i, j) * mult < 1)) Then
                    _map(i, j) = mult * _map(i, j)
                End If
                If mult = 0 AndAlso _map(i, j) <> 0 Then
                    _map(i, j) = 1
                End If
            Next
        Next
    End Sub

    Public Sub ScaleDown(minimum As Single)
        Dim mini As ULong = Min()
        If mini > minimum Then
            For i As Integer = 0 To _width - 1
                For j As Integer = 0 To _height - 1
                    _map(i, j) /= mini
                Next
            Next
        Else
            For i As Integer = 0 To _width - 1
                For j As Integer = 0 To _height - 1
                    _map(i, j) = (_map(i, j) + 0.0F) / minimum
                Next
            Next
        End If
    End Sub

    Public Sub Invert()
        Dim max As ULong = ULong.MinValue
        For Each i As ULong In _map
            max = Math.Max(max, i + 1)
        Next

        For i As Integer = 0 To _width - 1
            For j As Integer = 0 To _height - 1
                If _houseStatus(i + j * _width) <> HouseStatus.Hit Then
                    _map(i, j) = max - _map(i, j)
                End If
            Next
        Next
    End Sub

    Private Function GetStatus(x As Integer, y As Integer) As (horizontal As Byte, vertical As Byte, diagonal As Byte, miss As Byte)
        Dim countHorizontal As Byte = 0
        Dim countVertical As Byte = 0
        Dim countDiagonal As Byte = 0
        Dim countMiss As Byte = 0

        If x > 0 AndAlso _houseStatus(x - 1 + y * Width) = HouseStatus.Missed Then
            countMiss += 1
        End If

        If x < Width - 1 AndAlso _houseStatus(x + 1 + y * Width) = HouseStatus.Missed Then
            countMiss += 2
        End If

        If y > 0 AndAlso _houseStatus(x + (y - 1) * Width) = HouseStatus.Missed Then
            countMiss += 4
        End If

        If y < Height - 1 AndAlso _houseStatus(x + (y + 1) * Width) = HouseStatus.Missed Then
            countMiss += 8
        End If

        If x > 0 AndAlso y > 1 AndAlso _houseStatus(x - 1 + (y - 1) * Width) = HouseStatus.Missed Then
            countMiss += 16
        End If

        If x < Width - 1 AndAlso y > 1 AndAlso _houseStatus(x + 1 + (y - 1) * Width) = HouseStatus.Missed Then
            countMiss += 32
        End If

        If x > 0 AndAlso y < Height - 1 AndAlso _houseStatus(x - 1 + (y + 1) * Width) = HouseStatus.Missed Then
            countMiss += 64
        End If

        If x < Width - 1 AndAlso y < Height - 1 AndAlso _houseStatus(x + 1 + (y + 1) * Width) = HouseStatus.Missed Then
            countMiss += 128
        End If

        If x > 0 AndAlso _houseStatus((x - 1) + y * Width) = HouseStatus.Hit Then
            countHorizontal += 1
            If y > 0 AndAlso _houseStatus((x - 1) + (y - 1) * Width) = HouseStatus.Hit Then
                countDiagonal = countDiagonal Or 1
            End If
            If y < Height - 1 AndAlso _houseStatus((x - 1) + (y + 1) * Width) = HouseStatus.Hit Then
                countDiagonal = countDiagonal Or 2
            End If
        End If

        If x < Width - 1 AndAlso _houseStatus((x + 1) + y * Width) = HouseStatus.Hit Then
            countHorizontal += 1
            If y > 0 AndAlso _houseStatus((x + 1) + (y - 1) * Width) = HouseStatus.Hit Then
                countDiagonal = countDiagonal Or 4
            End If
            If y < Height - 1 AndAlso _houseStatus((x + 1) + (y + 1) * Width) = HouseStatus.Hit Then
                countDiagonal = countDiagonal Or 8
            End If
        End If


        If y > 0 AndAlso _houseStatus(x + (y - 1) * Width) = HouseStatus.Hit Then
            countVertical += 1
            If x > 0 AndAlso _houseStatus((x - 1) + (y - 1) * Width) = HouseStatus.Hit Then
                countDiagonal = countDiagonal Or 16
            End If
            If x < Width - 1 AndAlso _houseStatus((x + 1) + (y - 1) * Width) = HouseStatus.Hit Then
                countDiagonal = countDiagonal Or 32
            End If
        End If

        If y < Height - 1 AndAlso _houseStatus(x + (y + 1) * Width) = HouseStatus.Hit Then
            countVertical += 1
            If x > 0 AndAlso _houseStatus((x - 1) + (y + 1) * Width) = HouseStatus.Hit Then
                countDiagonal = countDiagonal Or 64
            End If
            If x < Width - 1 AndAlso _houseStatus((x + 1) + (y + 1) * Width) = HouseStatus.Hit Then
                countDiagonal = countDiagonal Or 128
            End If
        End If
        Return (countHorizontal, countVertical, countDiagonal, countMiss)
    End Function

    Private Function GetHouseMutiplierInvert(x As Integer, y As Integer) As Single
        If _houseStatus(x + y * Width) = HouseStatus.Hit Then
            Return 0
        End If

        Dim output As Single = 0
        Dim status As (horizontal As Byte, vertical As Byte, diagonal As Byte, miss As Byte) = GetStatus(x, y)

        If status.vertical = 0 AndAlso status.horizontal = 0 Then
            output = 8.0F
        ElseIf status.vertical = 0 OrElse status.horizontal = 0 Then
            output = 0.0F
        Else
            output = (status.horizontal + status.vertical) / 2.0F
        End If

        Return output
    End Function

    Private Function GetHorizontal(miss As Byte) As Integer
        Return 0 + ((miss And 1) = 1) + ((miss And 2) = 2)
    End Function

    Private Function GetVertical(miss As Byte) As Integer
        Return ((miss And 4) = 4) + ((miss And 8) = 8)
    End Function

    Private Function DetectLine(x As Integer, y As Integer) As Boolean
        If (x > 1 AndAlso _houseStatus(x - 1 + y * _width) = HouseStatus.Hit AndAlso _houseStatus(x - 2 + y * _width) = HouseStatus.Hit) OrElse
                (x < _width - 2 AndAlso _houseStatus(x + 1 + y * _width) = HouseStatus.Hit AndAlso _houseStatus(x + 2 + y * _width) = HouseStatus.Hit) OrElse
                (y > 1 AndAlso _houseStatus(x + (y - 1) * _width) = HouseStatus.Hit AndAlso _houseStatus(x + (y - 2) * _width) = HouseStatus.Hit) OrElse
                (y < _height - 2 AndAlso _houseStatus(x + (y + 1) * _width) = HouseStatus.Hit AndAlso _houseStatus(x + (y + 2) * _width) = HouseStatus.Hit) Then
            Return True
        End If
        Return False
    End Function

    Public Sub ExplicitlyAdd(explicitlyShip As (ship As Ship, position As (x As Integer, y As Integer), orientation As Orientation, complete As Boolean, weight As UInteger), toAdd As Integer)
        PutShip(explicitlyShip.position.x, explicitlyShip.position.y, explicitlyShip.ship, explicitlyShip.orientation, toAdd * explicitlyShip.weight)
    End Sub

    Public Sub ExplicitlyBlock(explicitlyShip As (ship As Ship, position As (x As Integer, y As Integer), orientation As Orientation, complete As Boolean, weight As UInteger))
        Dim size As (width As Integer, height As Integer) = GetSize(explicitlyShip.ship, explicitlyShip.orientation)

        For i As Integer = explicitlyShip.position.x To explicitlyShip.position.x + size.width - 1
            For j As Integer = explicitlyShip.position.y To explicitlyShip.position.y + size.height - 1
                _houseStatus(i + j * Width) = HouseStatus.Missed
            Next
        Next
    End Sub

    Public Sub AddMap(avaliable As (Battleship As UInteger, Carrier As UInteger, Destroyer As UInteger, Submarine As UInteger, Weight As UInteger))
        Dim orientations As Orientation() = New Orientation() {Orientation.Horizontal, Orientation.Vertical}

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
        If IsPuttable(x, y, ship, orientation, HouseStatus.Missed) Then
            Dim size As (width As Integer, height As Integer) = GetSize(ship, orientation)
            Dim weightExtra As Integer = GetCollisions(x, y, ship, orientation)

            For i As Integer = x To x + size.width - 1
                For j As Integer = y To y + size.height - 1
                    _map(i, j) = _map(i, j) + weight + weightExtra
                Next
            Next
        End If
    End Sub

    Private Sub MultiplyShip(x As Integer, y As Integer, ship As Ship, orientation As Orientation, multiply As Single)
        If IsPuttable(x, y, ship, orientation, HouseStatus.Missed) Then
            Dim size As (width As Integer, height As Integer) = GetSize(ship, orientation)

            For i As Integer = x To x + size.width - 1
                For j As Integer = y To y + size.height - 1
                    Try
                        _map(i, j) = _map(i, j) * multiply
                    Catch ex As Exception
                        ScaleDown(multiply)
                        _map(i, j) = _map(i, j) * multiply
                    End Try
                Next
            Next
        End If
    End Sub

    Private Function GetCollisions(x As Integer, y As Integer, ship As Ship, orientation As Orientation) As Integer
        Dim count As Integer = 0

        Dim size As (width As Integer, height As Integer) = GetSize(ship, orientation)
        For i As Integer = x To x + size.width - 1
            For j As Integer = y To y + size.height - 1
                If _houseStatus(i + j * _width) = HouseStatus.Hit Then
                    count += 1
                End If
            Next
        Next

        If count = 1 Then
            count = 2
        End If

        Return count
    End Function

    Public Function PutShip(x As Integer, y As Integer, ship As Ship, orientation As Orientation) As Boolean
        If IsPuttable(x, y, ship, orientation, HouseStatus.Hit) Then
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

    Private Function IsPuttable(x As Integer, y As Integer, ship As Ship, orientation As Orientation, housestatus As HouseStatus)
        Dim output As Boolean = True
        Dim size As (width As Integer, height As Integer) = GetSize(ship, orientation)

        output = IsFreeArea(x, y, size.width, size.height, housestatus)

        Return output
    End Function

    Private Function IsFreeArea(x As Integer, y As Integer, width As Integer, height As Integer, houseStatus As HouseStatus) As Boolean
        If x + width - 1 < Me.Width AndAlso y + height - 1 < Me.Height Then
            For i As Integer = x To x + width - 1
                For j As Integer = y To y + height - 1
                    If _houseStatus(i + j * Me.Width) = houseStatus Then
                        Return False
                    End If
                Next
            Next
        Else
            Return False
        End If
        Return True
    End Function

    Public Function IsAllAtSame() As Boolean
        Dim primary As ULong = 0
        For i = 0 To Width - 1
            For j = 0 To Height - 1
                If primary = 0 AndAlso _map(i, j) <> 0 Then
                    primary = _map(i, j)
                ElseIf primary <> _map(i, j) AndAlso _map(i, j) <> 0 Then
                    Return False
                End If
            Next
        Next
        Return True
    End Function

    ''' <summary>
    ''' Verifica se existe uma casa com valor maior que a media na porcetagem descrita por value.
    ''' </summary>
    ''' <param name="value"> Valorde 0.0F a 1.0F </param>
    ''' <returns></returns>
    Public Function ExistPercentageDiscrepancyValue(value As Single) As Boolean
        If value >= 0.0 AndAlso value <= 1.0F Then
            Dim avg As ULong = Me.AVG()
            For i = 0 To Width - 1
                For j = 0 To Height - 1
                    If _map(i, j) > avg AndAlso _map(i, j) - avg > (avg * value) Then
                        Return True
                    End If
                Next
            Next
        End If
        Return False
    End Function
End Structure
