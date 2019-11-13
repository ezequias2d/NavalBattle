''' <summary>
''' Modulo com algoritmo que junta coagulos em piscinas de coagulos
''' </summary>
Module CloatJoin
    Public Function Join(cloats As Cloat()) As CloatPool()
        Dim cloatPools As ICollection(Of CloatPool) = New List(Of CloatPool)

        If cloats.Length > 0 Then
            Dim pools As List(Of HashSet(Of Cloat)) = GeneratePools(cloats)

            For Each pool As HashSet(Of Cloat) In pools
                cloatPools.Add(CreateCloatPool(pool))
            Next
        End If

        Return cloatPools.ToArray()
    End Function

    Private Function CreateCloatPool(pool As HashSet(Of Cloat)) As CloatPool
        Dim minX As Integer = Integer.MaxValue
        Dim minY As Integer = Integer.MaxValue
        Dim maxX As Integer = Integer.MinValue
        Dim maxY As Integer = Integer.MinValue

        For Each cloat As Cloat In pool
            minX = Math.Min(cloat.Position.x, minX)
            minY = Math.Min(cloat.Position.y, minY)
            maxX = Math.Max(cloat.Position.x + cloat.Size.width, maxX)
            maxY = Math.Max(cloat.Position.y + cloat.Size.height, maxY)
        Next

        Dim width As Integer = maxX - minX
        Dim height As Integer = maxY - minY

        Dim map As Boolean(,) = New Boolean(width, height) {}

        For i As Integer = 0 To width - 1
            For j As Integer = 0 To height - 1
                For Each cloat As Cloat In pool
                    If IsBallot(cloat, (i + minX, j + minY)) Then
                        map(i, j) = True
                        Exit For
                    End If
                Next
            Next
        Next

        Return New CloatPool((minX, minY), map, (width, height))
    End Function

    ''' <summary>
    ''' Separa coagulos que podem forma uma piscina
    ''' </summary>
    ''' <param name="cloats"></param>
    ''' <returns></returns>
    Private Function GeneratePools(cloats As Cloat()) As List(Of HashSet(Of Cloat))
        Dim cloatsQueue As Queue(Of Cloat) = New Queue(Of Cloat)(cloats)
        Dim pools As List(Of HashSet(Of Cloat)) = New List(Of HashSet(Of Cloat))
        pools.Add(New HashSet(Of Cloat))

        pools(0).Add(cloatsQueue.Dequeue())

        While cloatsQueue.Count > 0

            Dim cloat1 As Cloat = cloatsQueue.Dequeue()
            Dim isAdd As Boolean = False
            Dim poolsToAdd As LinkedList(Of HashSet(Of Cloat)) = New LinkedList(Of HashSet(Of Cloat))

            For Each pool As HashSet(Of Cloat) In pools
                For Each cloat2 As Cloat In pool
                    If IsValidJoin(cloat1, cloat2) Then
                        isAdd = True
                        poolsToAdd.Append(pool)
                        Exit For
                    End If
                Next
            Next

            If poolsToAdd.Count = 1 Then
                ' So pertence a uma pool
                poolsToAdd.First().Value.Add(cloat1)
            ElseIf poolsToAdd.Count > 1 Then
                ' Pertence a mais de uma pool (uní as pools)
                Dim superPool As HashSet(Of Cloat) = New HashSet(Of Cloat)()
                For Each pool As HashSet(Of Cloat) In poolsToAdd
                    superPool.UnionWith(pool)
                    pools.Remove(pool)
                Next
                superPool.Add(cloat1)
                pools.Add(superPool)
            End If

            If (Not isAdd) Then
                Dim pool As HashSet(Of Cloat) = New HashSet(Of Cloat)
                pool.Add(cloat1)
                pools.Add(pool)
            End If
        End While

        Return pools
    End Function

    ''' <summary>
    ''' Verifica se os coagulos se colidem(AABB algorithm) / Forma uma piscina de coagulo
    ''' </summary>
    ''' <param name="cloat1"> Coagulo 1</param>
    ''' <param name="cloat2"> Coagulo 2 </param>
    ''' <returns></returns>
    Private Function IsValidJoin(cloat1 As Cloat, cloat2 As Cloat) As Boolean
        Return (((cloat1.Position.x <= cloat2.Position.x + cloat2.Size.width) AndAlso (cloat1.Position.x >= cloat2.Position.x) AndAlso
            (cloat1.Position.y <= cloat2.Position.y + cloat2.Size.height) AndAlso (cloat1.Position.y >= cloat2.Position.y)) OrElse
             ((cloat2.Position.x <= cloat1.Position.x + cloat1.Size.width) AndAlso (cloat2.Position.x >= cloat1.Position.x) AndAlso
            (cloat2.Position.y <= cloat1.Position.y + cloat1.Size.height) AndAlso (cloat2.Position.y >= cloat1.Position.y)))
    End Function

    ''' <summary>
    ''' Verifica se uma case está contida em um coagulo
    ''' </summary>
    ''' <param name="cloat"> Coagulo </param>
    ''' <param name="ballot"> Unidade </param>
    ''' <returns> Se pertence ao coagulo </returns>
    Private Function IsBallot(cloat As Cloat, ballot As (x As Integer, y As Integer)) As Boolean
        Return ballot.x >= cloat.Position.x AndAlso ballot.x < cloat.Position.x + cloat.Size.width AndAlso
            ballot.y >= cloat.Position.y AndAlso ballot.y < cloat.Position.y + cloat.Size.height
    End Function
End Module
