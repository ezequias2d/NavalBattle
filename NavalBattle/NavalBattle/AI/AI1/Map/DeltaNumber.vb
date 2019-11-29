Public Structure DeltaNumber

    Private min As UInteger
    Private max As UInteger

    Public Sub New(start As UInteger, size As UInteger)
        min = start
        max = start + size
    End Sub

    Public Function Inside(value As UInteger) As Boolean
        Return value <= max AndAlso value >= min
    End Function

    Public ReadOnly Property Final As UInteger
        Get
            Return max + 1
        End Get
    End Property

End Structure
