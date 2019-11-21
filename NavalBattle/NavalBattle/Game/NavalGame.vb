Public Class NavalGame
    Private _CurrentPlayer As PlayerID
    Private nextPlayer As PlayerID
    Private mapPlayer1 As Map
    Private mapPlayer2 As Map
    Private _LastHitted As Boolean
    Private _started As Boolean
    Public ForceViewPlayer2Map As Boolean

    Public ReadOnly Property LastHitted As Boolean
        Get
            Return _LastHitted
        End Get
    End Property

    Public Sub New(width As Integer, height As Integer, startPlayer As PlayerID)
        mapPlayer1 = New Map(width, height)
        mapPlayer2 = New Map(width, height)
        _LastHitted = False

        _CurrentPlayer = startPlayer
        Select Case startPlayer
            Case PlayerID.Player1
                nextPlayer = PlayerID.Player2
            Case PlayerID.Player2
                nextPlayer = PlayerID.Player1
        End Select
        _started = False
        ForceViewPlayer2Map = False
    End Sub

    Public Sub Start()
        _started = True
        Console.WriteLine("Start")
    End Sub

    Public Function IsEnd() As Boolean
        Return (mapPlayer1.Rest <= 0 OrElse mapPlayer2.Rest <= 0) And _started
    End Function

    Public Function GetWin() As PlayerID
        If mapPlayer1.Rest <= 0 AndAlso mapPlayer2.Rest > 0 Then
            Return PlayerID.Player2
        ElseIf mapPlayer1.Rest > 0 AndAlso mapPlayer2.Rest <= 0 Then
            Return PlayerID.Player1
        Else
            Return PlayerID.Undefined
        End If
    End Function

    Public Sub PutShip(x As Integer, y As Integer, ship As Ship, orientation As Orientation)
        Select Case CurrentPlayer
            Case PlayerID.Player1
                mapPlayer1.PutShip(x, y, ship, orientation)
            Case PlayerID.Player2
                mapPlayer2.PutShip(x, y, ship, orientation)
        End Select
    End Sub

    Public Function IsPuttable(x As Integer, y As Integer, ship As Ship, orientation As Orientation)
        Select Case CurrentPlayer
            Case PlayerID.Player1
                Return mapPlayer1.IsPuttable(x, y, ship, orientation)
            Case PlayerID.Player2
                Return mapPlayer2.IsPuttable(x, y, ship, orientation)
        End Select
        Return False
    End Function

    Public Sub Attack(x As Integer, y As Integer)
        Dim aux1 As Boolean = False
        Dim aux2 As Boolean = False
        Select Case CurrentPlayer
            Case PlayerID.Player1
                aux1 = mapPlayer2.IsHitted(x, y)
                mapPlayer2.Attack(x, y)
                aux2 = mapPlayer2.IsHitted(x, y)
            Case PlayerID.Player2
                aux1 = mapPlayer1.IsHitted(x, y)
                mapPlayer1.Attack(x, y)
                aux2 = mapPlayer1.IsHitted(x, y)
        End Select

        If (Not aux1) And aux2 Then
            _LastHitted = True
        Else
            _LastHitted = False
            SwapPlayer()
        End If
    End Sub

    Public Function IsAttackable(x As Integer, y As Integer) As Boolean
        Select Case CurrentPlayer
            Case PlayerID.Player1
                Return mapPlayer2.IsAttackable(x, y)
            Case PlayerID.Player2
                Return mapPlayer1.IsAttackable(x, y)
        End Select
        Return False
    End Function

    Public Sub SwapPlayer()
        Dim aux As PlayerID = CurrentPlayer
        CurrentPlayer = nextPlayer
        nextPlayer = aux
    End Sub

    Public Property CurrentPlayer As PlayerID
        Get
            Return _CurrentPlayer
        End Get
        Private Set
            _CurrentPlayer = Value
        End Set
    End Property

    Public Sub FillMap(ByRef map As NavalMap)
        If ForceViewPlayer2Map OrElse (CurrentPlayer = PlayerID.Player2 OrElse (_started = False)) Then
            Dim houses As House() = mapPlayer1.Houses()
            Dim enemyVisionMap As HouseStatus() = mapPlayer1.GetEnemyVisionMap()

            For i As Integer = 0 To mapPlayer1.Width - 1
                For j As Integer = 0 To mapPlayer1.Height - 1
                    Dim position As Integer = i + j * mapPlayer1.Width
                    map.SetHouse(i, j, houses(position).Ship, houses(position).Orientation, mapPlayer1.GetPiece(i, j))
                    map.SetHouse(i, j, enemyVisionMap(position))
                Next
            Next

        Else
            Dim enemyVisionMap As HouseStatus() = mapPlayer2.GetEnemyVisionMap()
            For i As Integer = 0 To mapPlayer2.Width - 1
                For j As Integer = 0 To mapPlayer2.Height - 1
                    Dim position As Integer = i + j * mapPlayer2.Width
                    map.SetHouse(i, j, Ship.None, Orientation.Horizontal, 0)
                    map.SetHouse(i, j, enemyVisionMap(position))
                Next
            Next
        End If
    End Sub

    Public Function GetEnemyVisionMap() As HouseStatus()
        Select Case CurrentPlayer
            Case PlayerID.Player1
                Return mapPlayer2.GetEnemyVisionMap()
            Case PlayerID.Player2
                Return mapPlayer1.GetEnemyVisionMap()
        End Select
        Return Nothing
    End Function

End Class
