Public Structure CloatPool

    Private _Position As (x As Integer, y As Integer)
    Private _Size As (width As Integer, height As Integer)
    Private _Map As Boolean(,)
    Private _count As Integer

    Public Sub New(Position As (x As Integer, y As Integer), mapCopy As Boolean(,), Size As (width As Integer, height As Integer))
        _Position = Position
        _Size = Size
        _Map = New Boolean(Size.width, Size.height) {}
        _count = 0
        For i As Integer = 0 To Size.width - 1
            For j As Integer = 0 To Size.height - 1
                _Map(i, j) = mapCopy(i, j)
                If _Map(i, j) Then
                    _count += 1
                End If
            Next
        Next
    End Sub

    Public Sub New(cloatPool As CloatPool)
        _Position = cloatPool.Position
        _Size = cloatPool.Size
        _Map = New Boolean(Size.width, Size.height) {}
        _count = 0
        For i As Integer = 0 To Size.width - 1
            For j As Integer = 0 To Size.height - 1
                _Map(i, j) = cloatPool.Item(i, j)
                If _Map(i, j) Then
                    _count += 1
                End If
            Next
        Next
    End Sub

    Public ReadOnly Property Position As (x As Integer, y As Integer)
        Get
            Return _Position
        End Get
    End Property

    Public ReadOnly Property Size As (width As Integer, height As Integer)
        Get
            Return _Size
        End Get
    End Property

    Public ReadOnly Property Count As Integer
        Get
            Return _count
        End Get
    End Property

    Public Property Item(x As Integer, y As Integer) As Boolean
        Get
            If x < Size.width AndAlso y < Size.height Then
                Return _Map(x, y)
            End If
            Return False
        End Get
        Set(value As Boolean)
            If x < Size.width AndAlso y < Size.height Then
                _Map(x, y) = value
            End If
        End Set
    End Property

    Public Sub Print()

        For j As Integer = 0 To Size.height - 1
            For i As Integer = 0 To Size.width - 1
                If Item(i, j) Then
                    Console.Write("X ")
                Else
                    Console.Write("_ ")
                End If
            Next
            Console.WriteLine()
        Next
    End Sub

    Public Sub PutShip(x As Integer, y As Integer, ship As Ship, orientation As Orientation)
        If IsPuttable(x, y, ship, orientation) Then
            Dim size As (width As Integer, height As Integer) = GetSize(ship, orientation)

            For i As Integer = x To x + size.width - 1
                For j As Integer = y To y + size.height - 1
                    Item(i, j) = False
                Next
            Next
            _count -= ship

        End If
    End Sub

    Public Function IsPuttable(x As Integer, y As Integer, ship As Ship, orientation As Orientation)
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

        If x + width - 1 < Size.width AndAlso y + height - 1 < Size.height Then
            For i As Integer = x To x + width - 1
                For j As Integer = y To y + height - 1
                    output = output AndAlso Item(i, j)
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
