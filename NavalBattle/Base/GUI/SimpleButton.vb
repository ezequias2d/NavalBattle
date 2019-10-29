Imports Microsoft.Xna.Framework
Imports Microsoft.Xna.Framework.Graphics
Imports NavalBattle

#Region "FILE DESCRIPTION"
''-----------------------------------------------------------------------------
'' SimpleButton.vb
''
'' Author: Ezequias Moises dos Santos Silva.
''-----------------------------------------------------------------------------
#End Region

''' <summary>
''' Botão simples
''' Possui um rotulo Label.
''' </summary>
Public Class SimpleButton
    Inherits GUIObject

    ''' <summary>
    ''' Frame de seleção
    ''' </summary>
    Private selectFrame As Frame

    ''' <summary>
    ''' Cria novo botão simples
    ''' </summary>
    ''' <param name="index"> Index unidimensional </param>
    ''' <param name="indexX"> Index da posição X </param>
    ''' <param name="indexY"> Index da posição Y </param>
    ''' <param name="position"> Posição no espaço </param>
    ''' <param name="selectFrame"> Frame de seleção </param>
    ''' <param name="label"> Label / Rótulo </param>
    Public Sub New(index As Integer, indexX As Integer, indexY As Integer, position As Vector2, selectFrame As Frame, label As Label)
        MyBase.New(index, indexX, indexY, position)
        Me.selectFrame = selectFrame
        Me.selectFrame.origin = New Vector2(selectFrame.source.Width, selectFrame.source.Height)
        Me.GUIObjectEnable = True

        Me.Origin = New Vector2(selectFrame.source.Width, selectFrame.source.Height) / 2.0F
        Me.Label = label
        Me.DrawEnable = True
        Me.Selected = False
        If label IsNot Nothing Then
            label.Position = position
        End If
    End Sub

    Public Sub New(index As Integer, indexX As Integer, indexY As Integer, position As Vector2, selectFrame As Frame)
        Me.New(index, indexX, indexY, position, selectFrame, Nothing)
    End Sub

    Public Property Label As Label

    Public Overrides Sub Update(gameTime As GameTime)
        MyBase.Update(gameTime)
    End Sub

    Public Overrides Sub Draw(ByRef spriteBatch As SpriteBatch, positionDelta As Vector2, scaleDelta As Vector2, angleDelta As Single, layerDepthDelta As UShort)
        Dim pos As Vector2 = PositionTranslated + selectFrame.origin * Scale + positionDelta

        If Label IsNot Nothing AndAlso Label.DrawEnable Then
            Label.Draw(spriteBatch)
        End If

        If Selected Then
            Dim scale1 As Vector2 = Scale * scaleDelta
            Dim scale2 As Vector2 = New Vector2(Scale.Y, Scale.X) * scaleDelta
            selectFrame.Draw(spriteBatch, pos, scale1, 0, LayerDetph)
            selectFrame.Draw(spriteBatch, pos, scale2, 90, LayerDetph)
            selectFrame.Draw(spriteBatch, pos, scale1, 180, LayerDetph)
            selectFrame.Draw(spriteBatch, pos, scale2, 270, LayerDetph)
        End If
    End Sub
End Class
