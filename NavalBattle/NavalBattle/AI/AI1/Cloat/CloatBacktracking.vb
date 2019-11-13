
''' <summary>
''' Modulo com algoritmo que varre em busca de um coagulo
''' </summary>
Module CloatBacktracking

    ''' <summary>
    ''' Analiza por Backtracking todas as combinações de linhas e colunas presas(Hit preso em volta de Miss) na horizontal e vertical
    ''' </summary>
    ''' <param name="houses"> Mapa </param>
    ''' <param name="width"> Largura </param>
    ''' <param name="height"> Altura </param>
    ''' <returns> Array de cloats na horizontal e vertical</returns>
    Public Function bk(houses As HouseStatus(), width As Integer, height As Integer) As Cloat()
        Dim cloats As ICollection(Of Cloat) = New List(Of Cloat)
        Dim a As (x As Integer, y As Integer)() = New(x As Integer, y As Integer)(width * height) {}

        bk_internal(houses, width, height, 0, Orientation.Horizontal, a, cloats)
        bk_internal(houses, width, height, 0, Orientation.Vertical, a, cloats)

        Return cloats.ToArray()
    End Function

    Private Sub bk_internal(houses As HouseStatus(), width As Integer, height As Integer, k As Integer, orientation As Orientation, ByRef a As (x As Integer, y As Integer)(), ByRef cloats As ICollection(Of Cloat))
        Dim c As (x As Integer, y As Integer)() = New(x As Integer, y As Integer)(width) {}
        Dim cNum As Integer = 0

        If isSolution(houses, width, height, a, k, orientation) Then
            Dim cloat As Cloat
            If orientation = Orientation.Horizontal Then
                cloat = New Cloat(a(0), (k, 1))
            Else
                cloat = New Cloat(a(0), (1, k))
            End If
            cloats.Add(cloat)
        Else
            k += 1

            GenereteCandidates(houses, width, height, a, k - 1, orientation, c, cNum)
            For i = 0 To cNum - 1
                a(k - 1) = c(i)
                bk_internal(houses, width, height, k, orientation, a, cloats)
            Next
        End If
    End Sub

    Private Sub GenereteCandidates(houses As HouseStatus(), width As Integer, height As Integer, a As (x As Integer, y As Integer)(), k As Integer, orientation As Orientation, ByRef c As (x As Integer, y As Integer)(), ByRef cNum As Integer)
        If k = 0 Then
            c = New(x As Integer, y As Integer)(width * height) {}
            For i As Integer = 0 To width - 1
                For j As Integer = 0 To height - 1
                    c(i + j * width) = (i, j)
                Next
            Next
            cNum = width * height
        Else
            Dim position As (x As Integer, y As Integer) = a(k - 1)
            If orientation = Orientation.Horizontal Then
                If position.x + 1 < width Then
                    c(0) = (position.x + 1, position.y)
                    cNum = 1
                End If
            ElseIf orientation = Orientation.Vertical Then
                If position.y + 1 < height Then
                    c(0) = (position.x, position.y + 1)
                    cNum = 1
                End If
            End If
        End If
    End Sub

    Private Function isSolution(houses As HouseStatus(), width As Integer, height As Integer, a As (x As Integer, y As Integer)(), k As Integer, orientation As Orientation) As Boolean
        Dim output As Boolean = False
        Dim barrier As Byte
        For i As Integer = 0 To k - 1
            barrier = GetBarrier(houses, width, height, a(i).x, a(i).y, orientation)
            If k = 1 Then
                ' tamanho é 1, só precisa verificar se a posição esta trancada
                output = (barrier = 15)
            ElseIf i = 0 Then
                ' inicio
                output = ((orientation = Orientation.Horizontal AndAlso (barrier = 11)) OrElse (orientation = Orientation.Vertical AndAlso (barrier = 7)))
            ElseIf i = k - 1 Then
                output = output AndAlso ((orientation = Orientation.Horizontal AndAlso (barrier = 14)) OrElse (orientation = Orientation.Vertical AndAlso (barrier = 13)))
            Else
                output = output AndAlso ((orientation = Orientation.Horizontal AndAlso (barrier = 10)) OrElse (orientation = Orientation.Vertical AndAlso (barrier = 5)))
            End If
        Next

        Return output
    End Function

    Private Function GetBarrier(houses As HouseStatus(), width As Integer, height As Integer, x As Integer, y As Integer, orientation As Orientation) As Byte
        If houses(x + y * width) <> HouseStatus.Hit Then
            Return 0
        End If

        Dim output As Byte = 0

        ' left
        If x - 1 < 0 OrElse houses(x - 1 + y * width) = HouseStatus.Missed OrElse (orientation = Orientation.Vertical AndAlso houses(x - 1 + y * width) <> HouseStatus.Normal) Then
            output += 1
        End If

        ' up
        If y - 1 < 0 OrElse houses(x + (y - 1) * width) = HouseStatus.Missed OrElse (orientation = Orientation.Horizontal AndAlso houses(x + (y - 1) * width) <> HouseStatus.Normal) Then
            output += 2
        End If

        ' right
        If x + 1 >= width OrElse houses(x + 1 + y * width) = HouseStatus.Missed OrElse (orientation = Orientation.Vertical AndAlso houses(x + 1 + y * width) <> HouseStatus.Normal) Then
            output += 4
        End If

        ' down
        If y + 1 >= height OrElse houses(x + (y + 1) * width) = HouseStatus.Missed OrElse (orientation = Orientation.Horizontal AndAlso houses(x - 1 + (y + 1) * width) <> HouseStatus.Normal) Then
            output += 8
        End If

        Return output
    End Function
End Module
