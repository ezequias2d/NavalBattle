Imports System.Threading

Module Resolver


    Public Function CalculatePossibleAvailableParts(map As HouseStatus(), width As UInteger, height As UInteger,
                                                    avaliable As (Battleship As UInteger,
                                                                    Carrier As UInteger,
                                                                    Destroyer As UInteger,
                                                                    Submarine As UInteger)) As IList(Of SubMap)

        Dim cloats As Cloat() = CloatBacktracking.bk(map, width, height)

        Dim cloatPools As CloatPool() = CloatJoin.Join(cloats)

        Dim aiMaps As SubMap()() = New SubMap(cloatPools.Length - 1)() {}

        If cloatPools.Length > 0 Then
            Dim queue As Queue(Of ThreadStart) = New Queue(Of ThreadStart)

            For i = 0 To (cloatPools.Length - 1)
                Dim index As Integer = i
                queue.Enqueue(Sub() SubMap.GenereteSubMap(aiMaps(index), map, width, height, cloatPools(index), avaliable.Battleship, avaliable.Carrier, avaliable.Destroyer, avaliable.Submarine))
            Next

            Dim th As Thread = New Thread(Sub() threadSubmap(queue))
            th.Name = "ThreadSubmap"
            th.Start()

            threadSubmap(queue)
            th.Join()
        End If

        Dim output As List(Of SubMap) = New List(Of SubMap)

        Dim a As SubMap() = New SubMap(aiMaps.Length) {}
        CalculatePossibleAvailablePartsIntern(map, width, height, aiMaps, a, 0, avaliable, output)

        Return output
    End Function

    Private Sub threadSubmap(queue As Queue(Of ThreadStart))
        While queue.Count > 0
            Dim t As ThreadStart = Nothing
            SyncLock queue
                If queue.Count > 0 Then
                    t = queue.Dequeue()
                End If
            End SyncLock

            If t IsNot Nothing Then
                t.Invoke()
            End If
        End While
    End Sub

    Private Function isSolution(a As SubMap(),
                                k As UInteger,
                                aiMaps As SubMap()(),
                                avaliable As (Battleship As UInteger, Carrier As UInteger, Destroyer As UInteger, Submarine As UInteger),
                                superSubMap As SubMap) As Boolean

        If k = aiMaps.Length AndAlso k <> 0 Then

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
                                                      ByRef output As ICollection(Of SubMap))

        Dim cNum As UInteger
        Dim c As SubMap()
        Dim superSubMap As SubMap = SubMap.Unify(a, k)
        If isSolution(a, k, aiMaps, avaliable, superSubMap) Then
            output.Add(superSubMap)
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
