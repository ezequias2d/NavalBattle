Class Resolver

    Private _cloatPools As CloatPool()
    Private _ships As Dictionary(Of Ship, Integer)
    Private _shipsKeys1 As Dictionary(Of Ship, UInteger)
    Private _shipsKeys2 As List(Of Ship)

    Public Sub New(cloatPools As CloatPool())
        _cloatPools = cloatPools
        _ships = New Dictionary(Of Ship, Integer)
        _shipsKeys1 = New Dictionary(Of Ship, UInteger)
        _shipsKeys2 = New List(Of Ship)

        _shipsKeys1.Add(Ship.Submarine, 0)
        _shipsKeys1.Add(Ship.Destroyer, 1)
        _shipsKeys1.Add(Ship.Battleship, 2)
        _shipsKeys1.Add(Ship.Carrier, 3)

        _shipsKeys2.Add(Ship.Submarine)
        _shipsKeys2.Add(Ship.Destroyer)
        _shipsKeys2.Add(Ship.Battleship)
        _shipsKeys2.Add(Ship.Carrier)
    End Sub

    Private Sub RunInternal(ByVal a As AI1Map(), remainingPieces As Integer(), ByRef maps As ICollection(Of Integer()))
        If IsFulled(a) = 0 Then
            maps.Add(remainingPieces)
            Return
        End If
        Dim orientations As Orientation() = New Orientation() {Orientation.Horizontal, Orientation.Vertical}
        For Each piece In _shipsKeys2
            For Each orientation In orientations

            Next
        Next
    End Sub

    Private Function IsFulled(maps As AI1Map()) As Boolean
        Dim output As Boolean = True
        For Each map In maps
            output = output AndAlso (map.Count = 0)
        Next
        Return output
    End Function
End Class
