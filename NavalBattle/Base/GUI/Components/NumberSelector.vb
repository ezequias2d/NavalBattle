Imports Microsoft.Xna.Framework
Imports Microsoft.Xna.Framework.Graphics

#Region "FILE DESCRIPTION"
''-----------------------------------------------------------------------------
'' NumberSelector.vb
''
'' Author: Ezequias Moises dos Santos Silva.
''-----------------------------------------------------------------------------
#End Region

''' <summary>
''' Seletor de numero
''' </summary>
Public Class NumberSelector
    Inherits GUIObject

    ''' <summary>
    ''' Valor minimo
    ''' </summary>
    Private minValue As Integer

    ''' <summary>
    ''' Valor maximo
    ''' </summary>
    Private maxValue As Integer

    ''' <summary>
    ''' Valor atual
    ''' </summary>
    ''' <returns> Valor </returns>
    Public Property Value As Integer
        Get
            Return _Value
        End Get
        Set(ByVal value As Integer)
            If value >= minValue AndAlso value <= maxValue Then
                label.Text = value.ToString()
                UpdateBoxSize()
                _Value = value
            End If
        End Set
    End Property

    ''' <summary>
    ''' Label que mostra o número na tela
    ''' </summary>
    Private label As Label
    Private box As Box
    Private boxSelect As Box
    Private _Value As Integer

    ''' <summary>
    ''' Contrutor de NumberSelector
    ''' </summary>
    ''' <param name="index"> Índice </param>
    ''' <param name="indexX"> Índice X</param>
    ''' <param name="indexY"> Índice Y </param>
    ''' <param name="position"> Posição no espaço </param>
    ''' <param name="minValue"> Valor mínimo </param>
    ''' <param name="maxValue"> Valor máximo </param>
    Public Sub New(index As Integer, indexX As Integer, indexY As Integer, position As Vector2, ByVal minValue As Integer, ByVal maxValue As Integer)
        MyBase.New(index, indexX, indexY, position)
        Me.minValue = minValue
        Me.maxValue = maxValue
        label = New Label(minValue.ToString(), Color.Black, Label.Font)

        box = New Box(Vector2.One, New Frame(GUIController.Texture, New Rectangle(48, 0, 24, 24)))
        boxSelect = New Box(Vector2.One, New Frame(GUIController.Texture, New Rectangle(72, 0, 24, 24)))
        Value = minValue
        OnHorizontal = AddressOf NumberSelector.HorizontalAxis
    End Sub

    Private Shared Sub HorizontalAxis(context As GUIContext, obj As GUIObject, axisValue As Single, axis As Axis)
        If obj.Focus AndAlso TypeOf obj Is NumberSelector Then
            Dim numberSelector As NumberSelector = TryCast(obj, NumberSelector)
            numberSelector.Value = numberSelector.Value + Math.Sign(axisValue)
        End If
    End Sub

    ''' <summary>
    ''' Chamado quando NumberSelector recebe foco.
    ''' 
    ''' Troca cor do texto para algo proximo do cinza para indicar o foco.
    ''' </summary>
    Public Overrides Sub Focused()
        label.Color = New Color(69, 69, 69, 255)
    End Sub

    ''' <summary>
    ''' Chamado quando NumberSelector perde o foco.
    ''' 
    ''' Troca cor do texto para preto(padrão) para indicar a falta de foco.
    ''' </summary>
    Public Overrides Sub Unfocused()
        label.Color = Color.Black
    End Sub

    ''' <summary>
    ''' Atualiza tamanho do box e boxSelect para acompanhar o tamanho do texto em label.
    ''' </summary>
    Private Sub UpdateBoxSize()
        box.Size = label.Measure(Scale) + Vector2.One * 8
        boxSelect.Size = box.Size
    End Sub

    ''' <summary>
    ''' Pega tamanho da caixa.
    ''' </summary>
    ''' <returns> Tamanho da caixa em um Vector2 </returns>
    Public Function GetSize() As Vector2
        Return box.Size
    End Function

    ''' <summary>
    ''' Desenha o NumberSelector
    ''' </summary>
    ''' <param name="spriteBatch"> Spritebatch </param>
    ''' <param name="positionDelta"> Posição adicional </param>
    ''' <param name="scaleDelta"> Escala adicional </param>
    ''' <param name="angleDelta"> Angulo adicional </param>
    ''' <param name="layerDepthDelta"> Camada de profundidade adicional </param>
    Public Overrides Sub Draw(ByRef spriteBatch As SpriteBatch, positionDelta As Vector2, scaleDelta As Vector2, angleDelta As Single, layerDepthDelta As UShort)
        If Selected Then
            boxSelect.Draw(spriteBatch, positionDelta + Position, scaleDelta * Scale, Angle + angleDelta, layerDepthDelta + LayerDetph + 1)
        Else
            box.Draw(spriteBatch, positionDelta + Position, scaleDelta * Scale, Angle + angleDelta, layerDepthDelta + LayerDetph + 1)
        End If

        label.Draw(spriteBatch, positionDelta + Position, scaleDelta * Scale, Angle + angleDelta, layerDepthDelta + LayerDetph + 2)
    End Sub
End Class
