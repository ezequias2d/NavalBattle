Imports Microsoft.Xna.Framework
Imports Microsoft.Xna.Framework.Graphics

Public Class Check
    Inherits GUIObject

    Private _value As Boolean

    ''' <summary>
    ''' Valor atual
    ''' </summary>
    ''' <returns> Valor </returns>
    Public Property Value As Boolean
        Get
            Return _value
        End Get
        Set(value As Boolean)

            _value = value
            UpdateBoxSizeAndLabel()

        End Set
    End Property

    ''' <summary>
    ''' Label que mostra o número na tela
    ''' </summary>
    Private label As Label
    Private box As Box
    Private boxSelect As Box

    Public Sub New(index As Integer, indexX As Integer, indexY As Integer, position As Vector2)
        MyBase.New(index, indexX, indexY, position)

        label = New Label("O", Color.Black, Label.Font)
        box = New Box(Vector2.One, New Frame(GUIController.Texture, New Rectangle(48, 0, 24, 24)))
        boxSelect = New Box(Vector2.One, New Frame(GUIController.Texture, New Rectangle(72, 0, 24, 24)))

        AxisModeSetting(Axis.Fire0) = AxisMode.Click

        OnFire0 = AddressOf Check.Fire0Axis

        Value = False
    End Sub

    Private Shared Sub Fire0Axis(context As GUIContext, obj As GUIObject, axisValue As Single, axis As Axis)

        Dim check As Check = TryCast(obj, Check)
        check.Value = Not check.Value

    End Sub

    ''' <summary>
    ''' Atualiza tamanho do box e boxSelect para acompanhar o tamanho do texto em label.
    ''' </summary>
    Private Sub UpdateBoxSizeAndLabel()
        If Value Then
            label.Text = "X"
            label.Color = Color.DarkGreen
        Else
            label.Text = "O"
            label.Color = Color.DarkRed
        End If
        box.Size = label.Measure(Scale) + Vector2.One * 8
        boxSelect.Size = box.Size
    End Sub

    Public Overrides Sub Draw(ByRef spriteBatch As SpriteBatch, positionDelta As Vector2, scaleDelta As Vector2, angleDelta As Single, layerDepthDelta As UShort)
        If Selected Then
            boxSelect.Draw(spriteBatch, positionDelta + Position, scaleDelta * Scale, Angle + angleDelta, layerDepthDelta + LayerDetph + 1)
        Else
            box.Draw(spriteBatch, positionDelta + Position, scaleDelta * Scale, Angle + angleDelta, layerDepthDelta + LayerDetph + 1)
        End If
        label.Draw(spriteBatch, positionDelta + Position, scaleDelta * Scale, Angle + angleDelta, layerDepthDelta + LayerDetph + 2)
    End Sub
End Class
