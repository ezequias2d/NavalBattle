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
End Structure
