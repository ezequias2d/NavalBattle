Public Module Picker


    Sub New()
        VBMath.Randomize()
    End Sub

    Public Function ToRaffle(Of T)(value1 As T, value2 As T)
        If VBMath.Rnd > 0.5F Then
            Return value1
        Else
            Return value2
        End If
    End Function

    Public Function ToRaffle(chanceMap As ChanceMap) As (x As Integer, y As Integer)
        Dim value As Single = VBMath.Rnd()
        Dim dic As Dictionary(Of (x As Integer, y As Integer), DeltaNumber) = New Dictionary(Of (x As Integer, y As Integer), DeltaNumber)

        Dim position As ULong = 0UL
        For i As Integer = 0 To chanceMap.Width - 1
            For j As Integer = 0 To chanceMap.Height - 1
                If chanceMap.Item(i, j) > 0 Then
                    Dim deltaNumber As DeltaNumber = New DeltaNumber(position, chanceMap.Item(i, j))
                    dic.Add((i, j), deltaNumber)
                    position = deltaNumber.Final
                End If
            Next
        Next

        position = value * position

        Dim output As (x As Integer, y As Integer)

        For Each pair As KeyValuePair(Of (x As Integer, y As Integer), DeltaNumber) In dic
            If pair.Value.Inside(position) Then
                output = pair.Key
            End If
        Next

        Return output
    End Function

End Module
