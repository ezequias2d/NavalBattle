Imports Microsoft.Xna.Framework
Imports Microsoft.Xna.Framework.Graphics

Public Class BackgroundMenu
    Inherits GUIObject

    Private naval2 As Texture2D
    Private internalDimensions As Vector2

    Private tilemapGround As Tilemap(Of Integer)
    Private tilemapPlants As Tilemap(Of Integer)
    Private anchor As Sprite
    Private anchorChain As Tilemap(Of Integer)

    Private logoSprite As AnimatedSprite(Of Integer)

    Public Property EnableLogo As Boolean

    Public Sub New(index As Integer, indexX As Integer, indexY As Integer, position As Vector2, naval2 As Texture2D, internalDimensions As Vector2)
        MyBase.New(index, indexX, indexY, position)
        Me.naval2 = naval2
        Me.internalDimensions = internalDimensions
        CreateLogoSprite()
        CreateTilemapPlants()
        CreateTilemapGround()
        CreateAnchor()
    End Sub

    Public Overrides Sub Update(gameTime As GameTime)
        MyBase.Update(gameTime)
        logoSprite.Update(gameTime)
    End Sub

    Private Sub CreateLogoSprite()
        logoSprite = New AnimatedSprite(Of Integer)()
        logoSprite.Position = New Vector2(0, -72)
        logoSprite.Scale = Vector2.One * 3
        Dim animationList As List(Of Frame) = New List(Of Frame)

        For i = 0 To 63
            VBMath.Randomize(Timer)
            animationList.Add(New Frame(naval2, Vector2.One * 16, New Rectangle(96 + (Math.Ceiling(VBMath.Rnd() * 3)) * 32, 0, 32, 32), Color.White, SpriteEffects.None))
        Next

        Dim animation As Animation = New Animation(animationList)
        logoSprite.UpdatesPerSecond = 10
        logoSprite.Animations.Add(0, animation)

        logoSprite.animation = 0
    End Sub

    Private Sub CreateTilemapGround()
        tilemapGround = New Tilemap(Of Integer)(internalDimensions.X / 8, 1)
        tilemapGround.Interval = Vector2.One * 8
        tilemapGround.LayerDepth = 2
        tilemapGround.Position = Vector2.UnitY * (internalDimensions.Y / 2 - 4.0F)

        For i = 0 To 3
            For j = 0 To 1
                Dim sprite As Sprite = New Sprite()
                sprite.Frame = New Frame(naval2, New Rectangle(80 + i * 8, 64 + j * 8, 8, 8))
                tilemapGround.Sprites.Add(i + j * 4, sprite)
            Next
        Next

        For i = 0 To internalDimensions.X / 8 - 1
            VBMath.Randomize(Timer)
            tilemapGround.SetIndex(i, 0, Math.Round(VBMath.Rnd() * 7))
        Next

    End Sub

    Private Sub CreateTilemapPlants()
        tilemapPlants = New Tilemap(Of Integer)(internalDimensions.X / 16, 1)
        tilemapPlants.Position = Vector2.UnitY * (internalDimensions.Y / 2 - 11.0F)
        tilemapPlants.LayerDepth = 2
        For i = 0 To 9
            Dim sprite As Sprite = New Sprite()
            sprite.Frame = New Frame(naval2, New Rectangle(64 + i * 16, 48, 16, 16))
            tilemapPlants.Sprites.Add(i, sprite)
        Next

        For i = 0 To internalDimensions.X / 16 - 1
            VBMath.Randomize(Timer)
            If VBMath.Rnd() < 0.25 Then
                VBMath.Randomize(Timer)
                tilemapPlants.SetIndex(i, 0, Math.Round(VBMath.Rnd() * 8))
            Else
                tilemapPlants.SetIndex(i, 0, 9)
            End If
        Next

    End Sub

    Private Sub CreateAnchor()
        anchor = New Sprite()
        anchor.LayerDepth = 3
        VBMath.Randomize(Timer)
        anchor.Position = Vector2.UnitY * (internalDimensions.Y / 2 - 19.0F) + Vector2.UnitX * (VBMath.Rnd * (internalDimensions.X - 32) - internalDimensions.X / 2)

        anchor.Frame = New Frame(naval2, New Rectangle(48, 64, 16, 16))

        anchorChain = New Tilemap(Of Integer)(1, Math.Ceiling(internalDimensions.Y / 8.0F))
        anchorChain.Interval = Vector2.One * 8
        anchorChain.LayerDepth = 3

        For i = 0 To 1
            For j = 0 To 1
                Dim sprite As Sprite = New Sprite()
                sprite.Frame = New Frame(naval2, New Rectangle(64 + i * 8, 64 + j * 8, 8, 8))
                anchorChain.Sprites.Add(i + j * 2, sprite)
            Next
        Next

        For i = 0 To Math.Ceiling(internalDimensions.Y / 8.0F) - 2
            VBMath.Randomize(Timer)
            anchorChain.SetIndex(0, i, Math.Round(VBMath.Rnd() * 2))
        Next
        anchorChain.SetIndex(0, Math.Ceiling(internalDimensions.Y / 8.0F) - 1, 3)

        anchorChain.Position = anchor.Position + New Vector2(12, internalDimensions.Y / 2 - Math.Ceiling(internalDimensions.Y / 8.0F) * 8 + 8)
    End Sub

    Public Overrides Sub Draw(ByRef spriteBatch As SpriteBatch, positionDelta As Vector2, scaleDelta As Vector2, angleDelta As Single, layerDepthDelta As UShort)
        If EnableLogo Then
            logoSprite.Draw(spriteBatch, Position + positionDelta, Scale * scaleDelta, Angle + angleDelta, layerDepthDelta + LayerDetph)
        End If
        tilemapGround.Draw(spriteBatch, Position + positionDelta, Scale * scaleDelta, Angle + angleDelta, layerDepthDelta + LayerDetph)
        anchor.Draw(spriteBatch, Position + positionDelta, Scale * scaleDelta, Angle + angleDelta, layerDepthDelta + LayerDetph)
        anchorChain.Draw(spriteBatch, Position + positionDelta, Scale * scaleDelta, Angle + angleDelta, layerDepthDelta + LayerDetph)
        tilemapPlants.Draw(spriteBatch, Position + positionDelta, Scale * scaleDelta, Angle + angleDelta, layerDepthDelta + LayerDetph)
    End Sub
End Class
