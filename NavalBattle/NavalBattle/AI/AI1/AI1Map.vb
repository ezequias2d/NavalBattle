Public Structure AI1Map
    Private cloatPool As CloatPool
    Private _ships As Dictionary(Of Ship, Integer)
    Private _count As UInteger

    Public Sub New(cloatPool As CloatPool)
        Me.cloatPool = cloatPool
        _count = cloatPool.Count
    End Sub

    Public ReadOnly Property Item(ship As Ship) As Integer
        Get
            If _ships.ContainsKey(ship) Then
                Return _ships(ship)
            End If
            _ships.Add(ship, 0)
            Return 0
        End Get
    End Property

    Public ReadOnly Property Count As UInteger
        Get
            Return _Count
        End Get
    End Property

    Public Function TryPutShip(x As Integer, y As Integer, ship As Ship, orientation As Orientation)
        Dim output As Boolean = True
        Dim size As (width As Integer, height As Integer) = GetSize(ship, orientation)

        output = IsFreeArea(x, y, size.width, size.height)

        If output Then
            For i As Integer = x To x + size.width - 1
                For j As Integer = y To y + size.height - 1
                    cloatPool.Item(i, j) = False
                Next
            Next
            _count -= ship
        End If

        Return output
    End Function

    Public Function GetSize(ship As Ship, orientation As Orientation) As (width As Integer, height As Integer)
        Dim size As (width As Integer, height As Integer)

        size.width = ship * Convert.ToInt32(orientation = Orientation.Horizontal) + Convert.ToInt32(orientation <> Orientation.Horizontal)
        size.height = ship * Convert.ToInt32(orientation = Orientation.Vertical) + Convert.ToInt32(orientation <> Orientation.Vertical)

        Return size
    End Function

    Private Function IsFreeArea(x As Integer, y As Integer, width As Integer, height As Integer) As Boolean
        Dim output As Boolean = True

        If x + width - 1 < cloatPool.Size.width AndAlso y + height - 1 < cloatPool.Size.height Then
            For i As Integer = x To x + width - 1
                For j As Integer = y To y + height - 1
                    output = output AndAlso cloatPool.Item(i, j)
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
