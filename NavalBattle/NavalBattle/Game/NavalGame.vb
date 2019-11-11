Public Class NavalGame
    Private _CurrentPlayer As PlayerID
    Private nextPlayer As PlayerID
    Private mapPlayer1 As Map
    Private mapPlayer2 As Map
    Private _LastHitted As Boolean

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
    End Sub

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

    Private Sub SwapPlayer()
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

        Dim houses As House() = mapPlayer1.Houses()
        For i As Integer = 0 To mapPlayer1.Width - 1
            For j As Integer = 0 To mapPlayer1.Height - 1
                Dim position As Integer = i + j * mapPlayer1.Width
                map.SetHouse(i, j, houses(position).Ship, houses(position).Orientation, mapPlayer1.GetPiece(i, j))
            Next
        Next

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
