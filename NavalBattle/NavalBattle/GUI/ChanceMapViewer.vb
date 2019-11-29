Imports Microsoft.Xna.Framework
Imports Microsoft.Xna.Framework.Graphics

Public Class ChanceMapViewer
    Inherits GUIObject

    Private texture As Texture2D
    Private sprite As Sprite

    Public Sub New(index As Integer, indexX As Integer, indexY As Integer, position As Vector2, width As Integer, height As Integer)
        MyBase.New(index, indexX, indexY, position)
        texture = ScreenManager.Instance.CreateTexture(width, height)
        sprite = New Sprite()
        sprite.Frame = New Frame(texture, New Rectangle(0, 0, width, height))
    End Sub

    Public Sub FillMap(chanceMap As ChanceMap)
        Dim data As Color() = New Color(texture.Width * texture.Height - 1) {}
        For xPixel As Integer = 0 To texture.Width - 1
            For yPixel As Integer = 0 To texture.Height - 1
                Dim cor As Byte = (255 * (chanceMap.Item(xPixel, yPixel) / 2)) / (chanceMap.Max / 2)

                data(xPixel + yPixel * texture.Width) = New Color(cor, cor, cor, 255)
            Next
        Next
        texture.SetData(data)
    End Sub

    Public Overrides Sub Draw(ByRef spriteBatch As SpriteBatch, positionDelta As Vector2, scaleDelta As Vector2, angleDelta As Single, layerDepthDelta As UShort)
        sprite.Draw(spriteBatch, Position + positionDelta, Scale * scaleDelta, Angle + angleDelta, LayerDetph + layerDepthDelta)
    End Sub
End Class
