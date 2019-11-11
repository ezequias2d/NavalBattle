Public Class Map
    Private _Houses As House()
    Private _Width As Integer
    Private _Height As Integer
    Private mask As Integer()
    Private maskCounter As Integer

    ''' <summary>
    ''' Cria novo mapa
    ''' </summary>
    ''' <param name="Width"> Largura </param>
    ''' <param name="Height"> Altura </param>
    Public Sub New(Width As Integer, Height As Integer)
        _Width = Width
        _Height = Height
        _Houses = New House(Width * Height) {}
        mask = New Integer(Width * Height) {}
        Clear()
    End Sub

    Public Function GetPiece(x As Integer, y As Integer) As Integer
        Dim position As Integer = x + y * Me.Width
        Dim house As House = _Houses(position)
        Dim previousX As Integer = x - Convert.ToInt32(house.Orientation = Orientation.Horizontal)
        Dim previousY As Integer = y - Convert.ToInt32(house.Orientation = Orientation.Vertical)
        Dim previousPosition As Integer = previousX + previousY * Me.Width

        If previousPosition < 0 Then
            Return 0
        ElseIf mask(position) <> mask(previousPosition) Then
            Return 0
        Else
            Return 1 + GetPiece(previousX, previousY)
        End If
    End Function

    ''' <summary>
    ''' Coloca embarcação no mapa, se possivel
    ''' </summary>
    ''' <param name="x"> Posição X </param>
    ''' <param name="y"> Posição Y </param>
    ''' <param name="ship"> Embarcação </param>
    ''' <param name="orientation"> Orientação </param>
    Public Sub PutShip(x As Integer, y As Integer, ship As Ship, orientation As Orientation)
        If Not IsPuttable(x, y, ship, orientation) Then
            Return
        End If

        Dim house As House = New House(ship, orientation, HouseStatus.Normal)
        Dim size As (width As Integer, height As Integer) = GetSize(ship, orientation)
        maskCounter = maskCounter + 1
        For i As Integer = x To x + size.width - 1
            For j As Integer = y To y + size.height - 1
                _Houses(i + j * Me.Width) = house
                mask(i + j * Me.Width) = maskCounter
            Next
        Next
    End Sub

    Public Sub PrintMap()
        Dim dicmap As Dictionary(Of Ship, String) = New Dictionary(Of Ship, String)()
        dicmap.Add(Ship.None, "_")
        dicmap.Add(Ship.Submarine, "S")
        dicmap.Add(Ship.Destroyer, "D")
        dicmap.Add(Ship.Battleship, "B")
        dicmap.Add(Ship.Carrier, "C")
        For i As Integer = 0 To Height - 1
            For j As Integer = 0 To Width - 1
                Dim house As House = _Houses(j + i * Width)
                Console.Write(dicmap(house.Ship) + " ")
            Next
            Console.WriteLine()
        Next
    End Sub

    ''' <summary>
    ''' Verifica se é possivel colocar embarcação
    ''' </summary>
    ''' <param name="x"> Posição X </param>
    ''' <param name="y"> Posição Y </param>
    ''' <param name="ship"> Embarcação </param>
    ''' <param name="orientation"> Orientação </param>
    ''' <returns></returns>
    Public Function IsPuttable(x As Integer, y As Integer, ship As Ship, orientation As Orientation)
        Dim output As Boolean = True
        Dim size As (width As Integer, height As Integer) = GetSize(ship, orientation)
        output = IsFreeArea(x, y, size.width, size.height)

        Return output
    End Function

    Public Shared Function GetSize(ship As Ship, orientation As Orientation) As (width As Integer, height As Integer)
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
                    output = output AndAlso (_Houses(i + j * Me.Width).Ship = Ship.None)
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

    ''' <summary>
    ''' Ataca casa(se for atacavel)
    ''' </summary>
    ''' <param name="x"> Posição X </param>
    ''' <param name="y"> Posição Y </param>
    Public Sub Attack(x As Integer, y As Integer)
        If Not IsAttackable(x, y) Then
            Return
        End If

        Dim position As Integer = x + y * Me.Width
        If _Houses(position).Ship <> Ship.None Then
            _Houses(position).Status = HouseStatus.Hit
        Else
            _Houses(position).Status = HouseStatus.Missed
        End If
    End Sub

    ''' <summary>
    ''' Verifica se casa é atacavel
    ''' </summary>
    ''' <param name="x"> Posição X </param>
    ''' <param name="y"> Posição Y </param>
    ''' <returns></returns>
    Public Function IsAttackable(x As Integer, y As Integer) As Boolean
        Return _Houses(x + y * Me.Width).Status = HouseStatus.Normal
    End Function

    Public Function IsHitted(x As Integer, y As Integer) As Boolean
        Return _Houses(x + y * Me.Width).Status = HouseStatus.Hit
    End Function

    Public Function GetEnemyVisionMap() As HouseStatus()
        Dim vision As HouseStatus() = New HouseStatus(_Houses.Count) {}

        For i As Integer = 0 To _Houses.Count - 1
            vision(i) = _Houses(i).Status
        Next

        Return vision
    End Function

    Public Sub Clear()
        Dim house As House = New House(Ship.None, Orientation.Horizontal, HouseStatus.Normal)
        For i As Integer = 0 To _Houses.Length - 1
            _Houses(i) = house
            mask(i) = 0
        Next
        maskCounter = 0
    End Sub

    Public ReadOnly Property Houses As House()
        Get
            Dim output As House() = New House(_Houses.Count) {}
            Array.Copy(_Houses, output, _Houses.Count)
            Return output
        End Get
    End Property

    Public ReadOnly Property Width As Integer
        Get
            Return _Width
        End Get
    End Property

    Public ReadOnly Property Height As Integer
        Get
            Return _Height
        End Get
    End Property


End Class
