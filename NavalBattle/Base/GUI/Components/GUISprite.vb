Imports Microsoft.Xna.Framework
Imports Microsoft.Xna.Framework.Graphics

''' <summary>
''' GUIObject que encapsula um Sprite
''' </summary>
Public Class GUISprite
    Inherits GUIObject

    Private _sprite As Sprite

    Public Sub New(index As Integer, indexX As Integer, indexY As Integer, position As Vector2, sprite As Sprite)
        MyBase.New(index, indexX, indexY, position)
        Me.sprite = sprite
    End Sub

    Public Overrides Sub Draw(ByRef spriteBatch As SpriteBatch, positionDelta As Vector2, scaleDelta As Vector2, angleDelta As Single, layerDepthDelta As UShort)
        If sprite.DrawEnable Then
            sprite.Draw(spriteBatch, Position + positionDelta, Scale * scaleDelta, Angle + angleDelta, LayerDetph + layerDepthDelta)
        End If
    End Sub

    Public Property Sprite As Sprite
        Get
            Return _sprite
        End Get
        Set(value As Sprite)
            _sprite = value
        End Set
    End Property
End Class
