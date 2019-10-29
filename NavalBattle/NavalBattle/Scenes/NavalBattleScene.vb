Imports Microsoft.Xna.Framework
Imports Microsoft.Xna.Framework.Graphics

Public Class NavalBattleScene
    Inherits GameScene

    Private naval As Texture2D


    Private sizeX As Integer
    Private sizeY As Integer

    Private navalMap As NavalMap
    Private font As SpriteFont

    Public Sub New(sizeX As Integer, sizeY As Integer)
        updates = New LinkedList(Of IUpdate)
        Me.sizeX = sizeX
        Me.sizeY = sizeY
    End Sub


    Public Overrides Sub LoadContent()
        MyBase.LoadContent()
        naval = content.Load(Of Texture2D)("naval")
        font = content.Load(Of SpriteFont)("fonts/PressStart2P")
        Dim area As Vector2 = New Vector2(Camera.InternalDimensions.X, Camera.InternalDimensions.Y - 16)
        navalMap = New NavalMap(GUIController.CurrentContext, naval, area, sizeX, sizeY)
        navalMap.Position = New Vector2(0, -8.0F)
    End Sub

End Class
