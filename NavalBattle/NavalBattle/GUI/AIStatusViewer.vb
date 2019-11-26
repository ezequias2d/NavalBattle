Imports Microsoft.Xna.Framework
Imports Microsoft.Xna.Framework.Graphics

Public Class AIStatusViewer
    Inherits GUIObject

    Private naval As Texture2D

    Private background As Sprite
    Private animationStatus As AnimatedSprite(Of AIStatus)
    Private ai As IAIPlayer

    Private animationRunning As Animation
    Private animationToRun As Animation
    Private animationToStop As Animation
    Private animationStopped As Animation

    Public Sub New(index As Integer, indexX As Integer, indexY As Integer, position As Vector2, naval As Texture2D, ai As IAIPlayer)
        MyBase.New(-2, -2, -2, position)
        Me.naval = naval
        Me.ai = ai

        background = New Sprite()
        background.Frame = New Frame(naval, New Vector2(8, 8), New Rectangle(32, 48, 16, 16), Color.White, SpriteEffects.None)
        background.LayerDepth = 0
        background.Scale = 2 * Vector2.One

        animationStatus = New AnimatedSprite(Of AIStatus)()
        animationStatus.Scale = 2 * Vector2.One
        animationStatus.LayerDepth = 1

        animationRunning = New Animation(New List(Of Frame))
        animationToRun = New Animation(New List(Of Frame))
        animationToStop = New Animation(New List(Of Frame))
        animationStopped = New Animation(New List(Of Frame))

        animationToRun.LoopEnable = False
        animationToStop.LoopEnable = False

        For i As Integer = 0 To 3
            For j As Integer = 0 To 2
                Dim frame As Frame = New Frame(naval, New Vector2(4, 4), New Rectangle(48 + 8 * i, 48 + 8 * j, 8, 8), Color.Wheat, SpriteEffects.None)
                If i >= 0 AndAlso i <= 2 AndAlso j = 0 Then
                    animationToRun.Frames.Add(frame)
                    animationToStop.Frames.Add(frame)
                ElseIf i = 3 AndAlso j = 2 Then
                    animationStopped.Frames.Add(frame)
                Else
                    animationRunning.Frames.Add(frame)
                End If
            Next
        Next

        animationToStop.Frames.Reverse()

        animationStatus.Animations.Add(AIStatus.Running, animationRunning)
        animationStatus.Animations.Add(AIStatus.Stopped, animationStopped)
        animationStatus.Animations.Add(AIStatus.ToRun, animationToRun)
        animationStatus.Animations.Add(AIStatus.ToStop, animationToStop)

        animationStatus.UpdatesPerSecond = 10
    End Sub

    Public Overrides Sub Update(gameTime As GameTime)

        If ai.IsInProcessing() Then
            If animationStatus.animation = AIStatus.Stopped Then
                animationStatus.animation = AIStatus.ToRun
            ElseIf animationStatus.animation = AIStatus.ToRun AndAlso animationStatus.Frame.texture Is Nothing Then
                animationToRun.Reset()
                animationStatus.animation = AIStatus.Running
            End If
        Else
            If animationStatus.animation = AIStatus.Running Then
                animationStatus.animation = AIStatus.ToStop
            ElseIf animationStatus.animation = AIStatus.ToStop AndAlso animationStatus.Frame.texture Is Nothing Then
                animationToStop.Reset()
                animationStatus.animation = AIStatus.Stopped
            End If
        End If
        animationStatus.Update(gameTime)
    End Sub

    Public Overrides Sub Draw(ByRef spriteBatch As SpriteBatch, positionDelta As Vector2, scaleDelta As Vector2, angleDelta As Single, layerDepthDelta As UShort)
        background.Draw(spriteBatch, positionDelta + Position, scaleDelta, angleDelta, layerDepthDelta)
        animationStatus.Draw(spriteBatch, positionDelta + Position, scaleDelta, angleDelta, layerDepthDelta + 1)
    End Sub

    Private Enum AIStatus
        Running
        Stopped
        ToStop
        ToRun
    End Enum
End Class
