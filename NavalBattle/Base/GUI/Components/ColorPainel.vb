Imports Microsoft.Xna.Framework
Imports Microsoft.Xna.Framework.Graphics

''' <summary>
''' Painel colorido simples
''' </summary>
Public Class ColorPainel
    Inherits GUIObject

    Private _size As Vector2
    Private _color As Color

    Public Property Color As Color
        Get
            Return _color
        End Get
        Set(value As Color)
            _color = value
        End Set
    End Property

    Public Property Size As Vector2
        Get
            Return _size
        End Get
        Set(value As Vector2)
            _size = value
        End Set
    End Property

    Public Sub New(index As Integer, indexX As Integer, indexY As Integer, position As Vector2, size As Vector2, color As Color)
        MyBase.New(index, indexX, indexY, position)
        Me.Color = color
        Me.Size = size
    End Sub

    Public Overrides Sub Draw(ByRef spriteBatch As SpriteBatch, positionDelta As Vector2, scaleDelta As Vector2, angleDelta As Single, layerDepthDelta As UShort)
        If PixelFrame.tint <> Color Then
            PixelFrame.tint = Color
        End If
        PixelFrame.Draw(spriteBatch, Position + positionDelta, Scale * scaleDelta * Size, Angle + angleDelta, LayerDetph + layerDepthDelta)
    End Sub

    Private Shared PixelFrame As Frame

    Shared Sub New()
        Dim texture As Texture2D = ScreenManager.Instance.CreateTexture(2, 2)
        PixelFrame = New Frame(texture, New Rectangle(0, 0, 1, 1))
    End Sub
End Class
