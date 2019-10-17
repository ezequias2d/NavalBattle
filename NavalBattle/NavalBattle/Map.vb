Public Class Map

    Private spawnedShips As IDictionary(Of Ship, Integer)

    Private ships As Ship()()
    Private mask As Integer()()
    Private statusMask As StatusHouse()()

    Private _X As Integer
    Private _Y As Integer
    Public Sub New(ByVal x As Integer, ByVal y As Integer)
        _X = x
        _Y = y
        spawnedShips = New Dictionary(Of Ship, Integer)

        ships = New Ship(x)() {}
        mask = New Integer(x)() {}
        statusMask = New StatusHouse(x)() {}
        For i = 0 To x - 1
            ships(i) = New Ship(y) {}
            mask(i) = New Integer(y) {}
            statusMask(i) = New StatusHouse(y) {}
        Next
        Reset()
    End Sub

    Public Sub Reset()
        For i = 0 To _X - 1
            For j = 0 To _Y - 1
                ships(i)(j) = Ship.None
                mask(i)(j) = 0
                statusMask(i)(j) = StatusHouse.None
            Next
        Next
    End Sub

    Public Function GetStatusMap() As StatusHouse()

    End Function

End Class
