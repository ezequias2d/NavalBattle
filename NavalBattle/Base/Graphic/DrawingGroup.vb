Imports Microsoft.Xna.Framework
Imports Microsoft.Xna.Framework.Graphics
Imports NavalBattle

#Region "FILE DESCRIPTION"
''-----------------------------------------------------------------------------
'' DrawingGroup.vb
''
'' Author: Ezequias Moises dos Santos Silva.
''-----------------------------------------------------------------------------
#End Region

''' <summary>
''' Coleção de um grupo de desenho
''' </summary>
Public Class DrawingGroup
    Implements ICollection(Of IDrawable)
    Implements IDrawable

    Private drawings As ICollection(Of IDrawable)

    Public Sub New()
        Me.drawings = New List(Of IDrawable)
    End Sub

    Public Sub New(drawings As ICollection(Of IDrawable))
        Me.drawings = drawings
    End Sub

    ''' <summary>
    ''' Numero de elementos na coleção
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property Count As Integer Implements ICollection(Of IDrawable).Count
        Get
            If drawings IsNot Nothing Then
                Return drawings.Count
            End If
            Return 0
        End Get
    End Property

    ''' <summary>
    ''' Obtém valor que indica se é somente leitura
    ''' </summary>
    ''' <returns> Se é somente leitura </returns>
    Public ReadOnly Property IsReadOnly As Boolean Implements ICollection(Of IDrawable).IsReadOnly
        Get
            If drawings IsNot Nothing Then
                Return drawings.IsReadOnly
            End If
            Return True
        End Get
    End Property

    ''' <summary>
    ''' Flag de desenho do DrawingGroup
    ''' </summary>
    ''' <returns> Se é para desenhar </returns>
    Public Property DrawEnable As Boolean Implements IDrawable.DrawEnable

    ''' <summary>
    ''' Camada de desenho que pertence.
    ''' Camadas podem ser ativadas ou desativadas da redenrização de uma camera especifica.
    ''' </summary>
    ''' <returns> Numero da camada </returns>
    Public Property Layer As Long Implements IDrawable.Layer

    ''' <summary>
    ''' Camada de sobreposição
    ''' </summary>
    ''' <returns> Camada que pertence </returns>
    Public Property LayerDetph As UShort Implements IDrawable.LayerDetph

    ''' <summary>
    ''' Copia os elementos para um System.Array, começando em um determinado índice do System.Array
    ''' </summary>
    ''' <param name="array"> Array </param>
    ''' <param name="arrayIndex"> Índice do array</param>
    Public Sub CopyTo(array() As IDrawable, arrayIndex As Integer) Implements ICollection(Of IDrawable).CopyTo
        If drawings IsNot Nothing Then
            drawings.CopyTo(array, arrayIndex)
        End If
    End Sub

    ''' <summary>
    ''' Adiciona item
    ''' </summary>
    ''' <param name="item"> Item </param>
    Public Sub Add(item As IDrawable) Implements ICollection(Of IDrawable).Add
        If drawings IsNot Nothing Then
            drawings.Add(item)
        End If
    End Sub

    ''' <summary>
    ''' Remove todos os itens
    ''' </summary>
    Public Sub Clear() Implements ICollection(Of IDrawable).Clear
        If drawings IsNot Nothing Then
            drawings.Clear()
        End If
    End Sub

    ''' <summary>
    ''' Função de desenho
    ''' Chama todos os elementos habilitados para desenho(DrawEnable) da coleção
    ''' </summary>
    ''' <param name="spriteBatch"> SpriteBatch para desenhar </param>
    ''' <param name="layerDepthDelta"> Camada adicional </param>
    Public Sub Draw(ByRef spriteBatch As SpriteBatch, ByVal layerDepthDelta As UShort) Implements IDrawable.Draw
        For Each element As IDrawable In drawings
            If element.DrawEnable Then
                element.Draw(spriteBatch, layerDepthDelta)
            End If
        Next
    End Sub

    ''' <summary>
    ''' Função de desenho
    ''' Chama todos os elementos habilitados para desenho(DrawEnable) da coleção
    ''' </summary>
    ''' <param name="spriteBatch"> SpriteBatch para desenhar </param>
    Public Sub Draw(ByRef spriteBatch As SpriteBatch) Implements IDrawable.Draw
        Draw(spriteBatch, 0)
    End Sub

    ''' <summary>
    ''' Função de desenho
    ''' Chama todos os elementos habilitados para desenho(DrawEnable) da coleção
    ''' </summary>
    ''' <param name="spriteBatch"> SpriteBatch para desenhar </param>
    ''' <param name="layerDepthDelta"> Camada adicional </param>
    ''' <param name="transformDelta"> Transform adicional </param>
    Public Sub Draw(ByRef spriteBatch As SpriteBatch, transformDelta As ITransform, layerDepthDelta As UShort) Implements IDrawable.Draw
        Draw(spriteBatch, transformDelta.Position, transformDelta.Scale, transformDelta.Angle, layerDepthDelta)
    End Sub

    ''' <summary>
    ''' Função de desenho
    ''' Chama todos os elementos habilitados para desenho(DrawEnable) da coleção
    ''' </summary>
    ''' <param name="spriteBatch"> SpriteBatch para desenhar </param>
    ''' <param name="positionDelta"> Posição adicional </param>
    ''' <param name="scaleDelta"> Escala adicional </param>
    ''' <param name="angleDelta"> Ângulo adicional </param>
    ''' <param name="layerDepthDelta"> Camada adicional </param>
    Public Sub Draw(ByRef spriteBatch As SpriteBatch, positionDelta As Vector2, scaleDelta As Vector2, angleDelta As Single, layerDepthDelta As UShort) Implements IDrawable.Draw
        For Each element As IDrawable In drawings
            If element.DrawEnable Then
                element.Draw(spriteBatch, positionDelta, scaleDelta, angleDelta, layerDepthDelta)
            End If
        Next
    End Sub

    ''' <summary>
    ''' Retorna um enumerador que itera pela coleção
    ''' </summary>
    ''' <returns> Enumerador </returns>
    Public Function GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
        If drawings IsNot Nothing Then
            Return drawings.GetEnumerator()
        End If
        Return Nothing
    End Function

    ''' <summary>
    ''' Determinará se contiver o valor específico.
    ''' </summary>
    ''' <param name="item"></param>
    ''' <returns></returns>
    Public Function Contains(item As IDrawable) As Boolean Implements ICollection(Of IDrawable).Contains
        If drawings IsNot Nothing Then
            Return drawings.Contains(item)
        End If
        Return False
    End Function

    ''' <summary>
    ''' Remove a primeira ocorrência de um objeto específico.
    ''' </summary>
    ''' <param name="item"> Objeto específico </param>
    ''' <returns> Se removeu </returns>
    Public Function Remove(item As IDrawable) As Boolean Implements ICollection(Of IDrawable).Remove
        If drawings IsNot Nothing Then
            Return drawings.Remove(item)
        End If
        Return False
    End Function

    Private Function IEnumerable_GetEnumerator() As IEnumerator(Of IDrawable) Implements IEnumerable(Of IDrawable).GetEnumerator
        If drawings IsNot Nothing Then
            Return drawings.GetEnumerator()
        End If
        Return Nothing
    End Function
End Class
