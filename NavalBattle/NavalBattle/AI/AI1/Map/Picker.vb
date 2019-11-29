Public Module Picker


    Sub New()
        VBMath.Randomize()
    End Sub

    Public Function ToRaffle(Of T)(value1 As T, value2 As T) As T
        VBMath.Randomize(Timer)
        If VBMath.Rnd > 0.5F Then
            Return value1
        Else
            Return value2
        End If
    End Function

    Public Function ToRaffle(chanceMap As ChanceMap) As (x As Integer, y As Integer)
        VBMath.Randomize(Timer)
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

    Public Function ToRaffle(chanceMap As ChanceMap, ship As Ship) As (x As Integer, y As Integer)
        Dim value As Single = VBMath.Rnd()
        Dim dic As Dictionary(Of (x As Integer, y As Integer), DeltaNumber) = New Dictionary(Of (x As Integer, y As Integer), DeltaNumber)

        Dim position As ULong = 0UL
        For i As Integer = 0 To chanceMap.Width - 1
            For j As Integer = 0 To chanceMap.Height - 1
                If chanceMap.Item(i, j) > 0 Then
                    Dim deltaNumber As DeltaNumber = New DeltaNumber(position, GetChanceShip(i, j, ship, chanceMap))
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

    Private Function GetChanceShip(x As Integer, y As Integer, ship As Ship, chanceMap As ChanceMap) As ULong
        Dim output As ULong = chanceMap.Item(x, y)

        For i As Integer = x To x + ship - 1
            If i < chanceMap.Width Then
                output = Math.Min(chanceMap.Item(i, y), output)
            End If
        Next

        For j As Integer = y To y + ship - 1
            If j < chanceMap.Height Then
                output = Math.Min(chanceMap.Item(x, j), output)
            End If
        Next

        Return output
    End Function

End Module
