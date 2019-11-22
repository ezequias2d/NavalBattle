Module Tools

    Private sizeDictionary As Dictionary(Of (ship As Ship, orientation As Orientation), (width As Integer, height As Integer))

    Sub New()
        sizeDictionary = New Dictionary(Of (ship As Ship, orientation As Orientation), (width As Integer, height As Integer))
    End Sub

    Public Function GetSize(ship As Ship, orientation As Orientation) As (width As Integer, height As Integer)
        Dim key As (Ship, Orientation) = (ship, orientation)
        If sizeDictionary.ContainsKey(key) Then
            Return sizeDictionary(key)
        Else
            Dim size As (width As Integer, height As Integer)

            size.width = ship * Convert.ToInt32(orientation = Orientation.Horizontal) + Convert.ToInt32(orientation <> Orientation.Horizontal)
            size.height = ship * Convert.ToInt32(orientation = Orientation.Vertical) + Convert.ToInt32(orientation <> Orientation.Vertical)

            sizeDictionary(key) = size

            Return size
        End If
    End Function

End Module
