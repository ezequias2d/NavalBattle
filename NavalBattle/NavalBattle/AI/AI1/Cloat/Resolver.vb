Module Resolver


    Public Function CalculatePossibleAvailableParts(map As HouseStatus(), width As UInteger, height As UInteger, avaliable As (Battleship As UInteger, Carrier As UInteger, Destroyer As UInteger, Submarine As UInteger)) As (Battleship As UInteger, Carrier As UInteger, Destroyer As UInteger, Submarine As UInteger, Weight As UInteger)()
        Dim cloats As Cloat() = CloatBacktracking.bk(map, width, height)

        Dim cloatPools As CloatPool() = CloatJoin.Join(cloats)

        Dim aiMaps As SubMap()() = New SubMap(cloatPools.Length - 1)() {}

        For i = 0 To cloatPools.Length - 1
            aiMaps(i) = SubMap.GenereteSubMap(cloatPools(i), avaliable.Battleship, avaliable.Carrier, avaliable.Destroyer, avaliable.Submarine)
        Next

        Dim output As List(Of (Battleship As UInteger, Carrier As UInteger, Destroyer As UInteger, Submarine As UInteger, Weight As UInteger)) = New List(Of (Battleship As UInteger, Carrier As UInteger, Destroyer As UInteger, Submarine As UInteger, Weight As UInteger))

        Dim a As SubMap() = New SubMap(aiMaps.Length) {}
        CalculatePossibleAvailablePartsIntern(aiMaps, a, 0, avaliable, output)

        If output.Count > 0 Then
            Return output.ToArray()
        Else
            Dim output2 As (Battleship As UInteger, Carrier As UInteger, Destroyer As UInteger, Submarine As UInteger, Weight As UInteger)()
            output2 = New(UInteger, UInteger, UInteger, UInteger, UInteger)() {(avaliable.Battleship, avaliable.Carrier, avaliable.Destroyer, avaliable.Submarine, 10)}
            Return output2
        End If
    End Function

    Private Function isSolution(a As SubMap(),
                                k As UInteger,
                                aiMaps As SubMap()(),
                                avaliable As (Battleship As UInteger, Carrier As UInteger, Destroyer As UInteger, Submarine As UInteger)) As Boolean

        If k >= aiMaps.Length Then
            Dim battleship As UInteger = 0
            Dim carrier As UInteger = 0
            Dim destroyer As UInteger = 0
            Dim submarine As UInteger = 0
            For i = 0 To k - 1
                battleship += a(i).Battleship
                carrier += a(i).Carrier
                destroyer += a(i).Destroyer
                submarine += a(i).Submarine
            Next
            Return battleship <= avaliable.Battleship AndAlso
                carrier <= avaliable.Carrier AndAlso
                destroyer <= avaliable.Destroyer AndAlso
                submarine <= avaliable.Submarine
        End If
        Return False
    End Function

    Private Function Total(a As SubMap(), k As UInteger) As (Battleship As UInteger, Carrier As UInteger, Destroyer As UInteger, Submarine As UInteger, Weight As UInteger)
        Dim battleship As UInteger = 0
        Dim carrier As UInteger = 0
        Dim destroyer As UInteger = 0
        Dim submarine As UInteger = 0
        Dim weight As Double = 1.0
        For i = 0 To k - 1
            battleship += a(i).Battleship
            carrier += a(i).Carrier
            destroyer += a(i).Destroyer
            submarine += a(i).Submarine

            If i = 0 Then
                weight = Math.Max(a(i).Weight, 1)
            Else
                weight = (weight + a(i).Weight) * i / (i + 1)
            End If
        Next
        Return (battleship, carrier, destroyer, submarine, Math.Max(1.0, weight))
    End Function

    Private Function ContainsSolution(collection As ICollection(Of (Battleship As UInteger, Carrier As UInteger, Destroyer As UInteger, Submarine As UInteger, Weight As UInteger)),
                                      solution As (Battleship As UInteger, Carrier As UInteger, Destroyer As UInteger, Submarine As UInteger, Weight As UInteger)) As Boolean
        For Each other As (Battleship As UInteger, Carrier As UInteger, Destroyer As UInteger, Submarine As UInteger, Weight As UInteger) In collection
            If other.Battleship = solution.Battleship AndAlso other.Carrier = solution.Carrier AndAlso other.Destroyer = solution.Destroyer AndAlso other.Submarine = solution.Submarine Then
                Return True
            End If
        Next
        Return False
    End Function

    Private Sub CalculatePossibleAvailablePartsIntern(aiMaps As SubMap()(), ByRef a As SubMap(), k As UInteger,
                                                      avaliable As (Battleship As UInteger, Carrier As UInteger, Destroyer As UInteger, Submarine As UInteger),
                                                      ByRef output As ICollection(Of (Battleship As UInteger, Carrier As UInteger, Destroyer As UInteger, Submarine As UInteger, Weight As UInteger)))

        Dim cNum As UInteger = aiMaps.Length
        Dim c As SubMap() = New SubMap(cNum) {}

        If isSolution(a, k, aiMaps, avaliable) Then
            Dim aux As (Battleship As UInteger, Carrier As UInteger, Destroyer As UInteger, Submarine As UInteger, Weight As UInteger) = Total(a, k)
            Dim solution As (Battleship As UInteger, Carrier As UInteger, Destroyer As UInteger, Submarine As UInteger, Weight As UInteger)
            solution = (avaliable.Battleship - aux.Battleship, avaliable.Carrier - aux.Carrier, avaliable.Destroyer - aux.Destroyer, avaliable.Submarine - aux.Submarine, aux.Weight)

            If Not ContainsSolution(output, solution) Then
                output.Add(solution)
            Else
                For Each other In output
                    If other.Battleship = solution.Battleship AndAlso other.Carrier = solution.Carrier AndAlso other.Destroyer = solution.Destroyer AndAlso other.Submarine = solution.Submarine Then
                        solution.Weight += other.Weight
                        output.Remove(other)
                        output.Add(solution)
                        Exit For
                    End If
                Next
            End If
        Else
            k += 1

            If k <= aiMaps.Length Then
                GenereteCandidates(aiMaps, k, c, cNum)
                For i = 0 To cNum - 1
                    a(k - 1) = c(i)
                    CalculatePossibleAvailablePartsIntern(aiMaps, a, k, avaliable, output)
                Next
            End If
        End If
    End Sub

    Private Sub GenereteCandidates(aiMaps As SubMap()(), k As UInteger, ByRef c As SubMap(), ByRef cNum As UInteger)

        For i As Integer = 0 To aiMaps(k - 1).Length - 1
            c(i) = aiMaps(k - 1)(i)
        Next
        cNum = aiMaps(k - 1).Length
    End Sub

End Module
