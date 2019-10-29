Imports NavalBattle

#Region "FILE DESCRIPTION"
''-----------------------------------------------------------------------------
'' AxisEqualityComparer.vb
''
'' Author: Ezequias Moises dos Santos Silva.
''-----------------------------------------------------------------------------
#End Region

''' <summary>
''' Comparador de equivalencia entre IAxis
''' </summary>
Public Class AxisEqualityComparer
    Implements IEqualityComparer(Of IAxis)

    Public Overloads Function Equals(x As IAxis, y As IAxis) As Boolean Implements IEqualityComparer(Of IAxis).Equals

        If x Is Nothing And y Is Nothing Then
            Return True
        ElseIf x Is Nothing Or y Is Nothing Then
            Return False
        ElseIf x.GetType().Equals(y.GetType()) Then
            If TypeOf x Is AxisKeyboard Then
                Dim xKeyboard As AxisKeyboard = x
                Dim yKeyboard As AxisKeyboard = y
                Return xKeyboard.Positive = yKeyboard.Positive AndAlso xKeyboard.Negative = yKeyboard.Negative
            ElseIf TypeOf x Is AxisMouse Then
                Dim xMouse As AxisMouse = x
                Dim yMouse As AxisMouse = y
                Return xMouse.Positive = yMouse.Positive AndAlso xMouse.Negative = yMouse.Negative
            ElseIf TypeOf x Is AxisGamepad Then
                Dim xGamepad As AxisGamepad = x
                Dim yGamepad As AxisGamepad = y
                Return xGamepad.Positive = yGamepad.Positive AndAlso xGamepad.Negative = yGamepad.Negative
            Else
                Return False
            End If
        Else
            Return False
        End If
    End Function

    Public Overloads Function GetHashCode(obj As IAxis) As Integer Implements IEqualityComparer(Of IAxis).GetHashCode
        Return obj.GetHashCode()
    End Function
End Class
