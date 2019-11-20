Module Resolver


    Public Function CalculatePossibleAvailableParts(map As HouseStatus(), width As UInteger, height As UInteger, avaliable As (Battleship As UInteger, Carrier As UInteger, Destroyer As UInteger, Submarine As UInteger)) As (Battleship As UInteger, Carrier As UInteger, Destroyer As UInteger, Submarine As UInteger, Weight As UInteger)()
        Dim cloats As Cloat() = CloatBacktracking.bk(map, width, height)

        Dim cloatPools As CloatPool() = CloatJoin.Join(cloats)

        Dim aiMaps As SubMap()() = New SubMap(cloatPools.Length - 1)() {}

        For i = 0 To cloatPools.Length - 1
            aiMaps(i) = SubMap.GenereteSubMap(map, width, height, cloatPools(i), avaliable.Battleship, avaliable.Carrier, avaliable.Destroyer, avaliable.Submarine)
        Next

        Dim output As List(Of (Battleship As UInteger, Carrier As UInteger, Destroyer As UInteger, Submarine As UInteger, Weight As UInteger)) = New List(Of (Battleship As UInteger, Carrier As UInteger, Destroyer As UInteger, Submarine As UInteger, Weight As UInteger))

        Dim a As SubMap() = New SubMap(aiMaps.Length) {}
        CalculatePossibleAvailablePartsIntern(map, width, height, aiMaps, a, 0, avaliable, output)

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

        If k = aiMaps.Length AndAlso k <> 0 Then
            Dim superSubMap As SubMap = SubMap.Unify(a, k)

            Dim output As Boolean = (superSubMap.Battleship <= avaliable.Battleship) AndAlso
                (superSubMap.Carrier <= avaliable.Carrier) AndAlso
                (superSubMap.Destroyer <= avaliable.Destroyer) AndAlso
                (superSubMap.Submarine <= avaliable.Submarine)
            Return output
        End If
        Return False
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



    Private Sub CalculatePossibleAvailablePartsIntern(map As HouseStatus(), width As UInteger, height As UInteger, aiMaps As SubMap()(), ByRef a As SubMap(), k As UInteger,
                                                      avaliable As (Battleship As UInteger, Carrier As UInteger, Destroyer As UInteger, Submarine As UInteger),
                                                      ByRef output As ICollection(Of (Battleship As UInteger, Carrier As UInteger, Destroyer As UInteger, Submarine As UInteger, Weight As UInteger)))

        Dim cNum As UInteger
        Dim c As SubMap()

        If isSolution(a, k, aiMaps, avaliable) Then
            Dim total As SubMap = SubMap.Unify(a, k)
            Dim solution As (Battleship As UInteger, Carrier As UInteger, Destroyer As UInteger, Submarine As UInteger, Weight As UInteger) = (avaliable.Battleship, avaliable.Carrier, avaliable.Destroyer, avaliable.Submarine, total.Weight)
            For Each detail As (ship As Ship, position As (x As Integer, y As Integer), orientation As Orientation, complete As Boolean) In total.Details

                If detail.complete Then
                    Select Case detail.ship
                        Case Ship.Battleship
                            solution.Battleship -= 1
                        Case Ship.Carrier
                            solution.Carrier -= 1
                        Case Ship.Destroyer
                            solution.Destroyer -= 1
                        Case Ship.Submarine
                            solution.Submarine -= 1
                    End Select
                End If
            Next

            If Not ContainsSolution(output, solution) Then
                output.Add(solution)
            Else
                Dim weight As ULong = 0
                For Each other In output
                    If other.Battleship = solution.Battleship AndAlso other.Carrier = solution.Carrier AndAlso other.Destroyer = solution.Destroyer AndAlso other.Submarine = solution.Submarine Then
                        weight += other.Weight
                        output.Remove(other)
                        output.Add(solution)
                        Exit For
                    End If
                Next
                solution.Weight += weight / output.LongCount
            End If
        Else
            k += 1

            If k <= aiMaps.Length Then
                GenereteCandidates(aiMaps, k, c, cNum)
                For i = 0 To cNum - 1
                    a(k - 1) = c(i)
                    CalculatePossibleAvailablePartsIntern(map, width, height, aiMaps, a, k, avaliable, output)
                Next
            End If
        End If
    End Sub

    Private Sub GenereteCandidates(aiMaps As SubMap()(), k As UInteger, ByRef c As SubMap(), ByRef cNum As UInteger)

        c = New SubMap(aiMaps(k - 1).Length) {}
        cNum = aiMaps(k - 1).Length
        For i As Integer = 0 To aiMaps(k - 1).Length - 1
            c(i) = aiMaps(k - 1)(i)
        Next
    End Sub

End Module
