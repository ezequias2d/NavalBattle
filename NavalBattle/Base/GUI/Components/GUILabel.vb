Imports Microsoft.Xna.Framework
Imports Microsoft.Xna.Framework.Graphics

Public Class GUILabel
    Inherits GUIObject

    Public Property Label As Label

    Public Sub New(index As Integer, indexX As Integer, indexY As Integer, position As Vector2, label As Label)
        MyBase.New(index, indexX, indexY, position)
        Me.Label = label
        Me.GUIObjectEnable = False
    End Sub

    Public Overrides Sub Draw(ByRef spriteBatch As SpriteBatch, positionDelta As Vector2, scaleDelta As Vector2, angleDelta As Single, layerDepthDelta As UShort)
        If Label.DrawEnable Then
            Label.Draw(spriteBatch, Position + positionDelta, Scale * scaleDelta, Angle + angleDelta, LayerDetph + layerDepthDelta)
        End If
    End Sub
End Class
