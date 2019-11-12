Public Structure Cloat

    Private _Position As (x As Integer, y As Integer)
    Private _Size As (width As Integer, height As Integer)

    Public Sub New(Position As (x As Integer, y As Integer), Size As (width As Integer, height As Integer))
        _Position = Position
        _Size = Size
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
End Structure
