Imports Microsoft.Xna.Framework
Imports Microsoft.Xna.Framework.Graphics

Public Class SimpleAlternator(Of T)
    Inherits GUIObject

    Private _count As UInteger
    Private alternative As IDictionary(Of String, T)

    ''' <summary>
    ''' Valor atual
    ''' </summary>
    ''' <returns> Valor </returns>
    Public Property Value As T
        Get
            Return alternative(Text)
        End Get
        Set(value As T)
            Dim c As UInteger = 0
            For Each Val As T In alternative.Values
                If Val.Equals(value) Then
                    Count = c
                End If
                c += 1
            Next
        End Set
    End Property

    Public Property Count As UInteger
        Get
            Return _count
        End Get
        Set(value As UInteger)
            _count = value
        End Set
    End Property

    Public ReadOnly Property Text As String
        Get
            Return alternative.Keys(count)
        End Get
    End Property

    Public Property GetStringFunction As Label.GetString
        Get
            Return label.GetStringFunction
        End Get
        Set(value As Label.GetString)
            label.GetStringFunction = value
        End Set
    End Property

    ''' <summary>
    ''' Label que o rotulo
    ''' </summary>
    Private label As Label
    Private box As Box
    Private boxSelect As Box
    Private _Value As Integer


    Public Sub New(index As Integer, indexX As Integer, indexY As Integer, position As Vector2, alternatives As IDictionary(Of String, T))
        MyBase.New(index, indexX, indexY, position)

        alternative = New Dictionary(Of String, T)(alternatives)

        label = New Label(Text, Color.Black, Label.Font)
        box = New Box(Vector2.One, New Frame(GUIController.Texture, New Rectangle(48, 0, 24, 24)))
        boxSelect = New Box(Vector2.One, New Frame(GUIController.Texture, New Rectangle(72, 0, 24, 24)))

        OnHorizontal = AddressOf SimpleAlternator(Of T).HorizontalAxis

        Count = 0
    End Sub

    Private Shared Sub HorizontalAxis(context As GUIContext, obj As GUIObject, axisValue As Single, axis As Axis)
        If obj.Focus AndAlso TypeOf obj Is SimpleAlternator(Of T) Then
            Dim simpleAlternator As SimpleAlternator(Of T) = TryCast(obj, SimpleAlternator(Of T))
            If simpleAlternator.Count = 0 AndAlso Math.Sign(axisValue) = -1 Then
                simpleAlternator.Count = simpleAlternator.alternative.Count - 1
            ElseIf simpleAlternator.Count = simpleAlternator.alternative.Count - 1 AndAlso Math.Sign(axisValue) = 1 Then
                simpleAlternator.Count = 0
            Else
                simpleAlternator.Count = simpleAlternator.Count + Math.Sign(axisValue)
            End If
        End If
    End Sub

    ''' <summary>
    ''' Atualiza tamanho do box e boxSelect para acompanhar o tamanho do texto em label.
    ''' </summary>
    Private Sub UpdateBoxSizeAndLabel()
        label.Text = Text
        box.Size = label.Measure(Scale) + Vector2.One * 8
        boxSelect.Size = box.Size
    End Sub

    ''' <summary>
    ''' Chamado quando NumberSelector recebe foco.
    ''' 
    ''' Troca cor do texto para algo proximo do cinza para indicar o foco.
    ''' </summary>
    Public Overrides Sub Focused()
        Label.Color = New Color(69, 69, 69, 255)
    End Sub

    ''' <summary>
    ''' Chamado quando NumberSelector perde o foco.
    ''' 
    ''' Troca cor do texto para preto(padrão) para indicar a falta de foco.
    ''' </summary>
    Public Overrides Sub Unfocused()
        label.Color = Color.Black
    End Sub

    Public Overrides Sub Draw(ByRef spriteBatch As SpriteBatch, positionDelta As Vector2, scaleDelta As Vector2, angleDelta As Single, layerDepthDelta As UShort)
        UpdateBoxSizeAndLabel()
        If Selected Then
            boxSelect.Draw(spriteBatch, positionDelta + Position, scaleDelta * Scale, Angle + angleDelta, layerDepthDelta + LayerDetph + 1)
        Else
            box.Draw(spriteBatch, positionDelta + Position, scaleDelta * Scale, Angle + angleDelta, layerDepthDelta + LayerDetph + 1)
        End If
        label.Draw(spriteBatch, positionDelta + Position, scaleDelta * Scale, Angle + angleDelta, layerDepthDelta + LayerDetph + 2)
    End Sub
End Class
