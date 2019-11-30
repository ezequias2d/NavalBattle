Imports Microsoft.Xna.Framework
Imports Microsoft.Xna.Framework.Graphics

Public Class ControlsViewer
    Inherits GUIObject

    Private list As List(Of (labelName As Label, controls As IList(Of (icon As Sprite, iconLabel As Label))))

    Public Sub New(index As Integer, indexX As Integer, indexY As Integer, position As Vector2)
        MyBase.New(index, indexX, indexY, position)
        list = New List(Of (labelName As Label, controls As IList(Of (icon As Sprite, iconLabel As Label))))
    End Sub

    Public Sub Add(labelName As Label, controls As IList(Of (icon As Sprite, iconLabel As Label)))
        Dim item As (labelName As Label, IList(Of (icon As Sprite, iconLabel As Label)))
        item = (labelName, controls)
        labelName.Position = Vector2.Zero
        Dim lastPosition As Vector2 = Vector2.Zero
        For Each control In controls
            lastPosition = lastPosition - New Vector2(labelName.Measure().Y, 0)

            If control.icon IsNot Nothing Then
                control.icon.Position = lastPosition
                control.icon.Scale = (Vector2.One * labelName.Measure().Y) / New Vector2(control.icon.Frame.source.Width, control.icon.Frame.source.Height)
            End If

            If control.iconLabel IsNot Nothing Then
                control.iconLabel.Scale = (Vector2.One * (labelName.Measure().Y - 1)) / control.iconLabel.Measure()
                control.iconLabel.Position = lastPosition + Vector2.One
            End If

        Next
        list.Add(item)
    End Sub

    Public Overrides Sub Draw(ByRef spriteBatch As SpriteBatch, positionDelta As Vector2, scaleDelta As Vector2, angleDelta As Single, layerDepthDelta As UShort)
        Dim newPosition As Vector2 = New Vector2(0, -GetLength(scaleDelta))
        For Each element In list
            If element.labelName.DrawEnable Then
                element.labelName.Draw(spriteBatch, (Position + newPosition) * scaleDelta + positionDelta, scaleDelta * Scale, angleDelta + Angle, LayerDetph + layerDepthDelta)
                For Each control In element.controls
                    If control.icon IsNot Nothing Or control.iconLabel IsNot Nothing Then
                        Dim translate As Vector2 = (Position + newPosition) * scaleDelta + positionDelta - New Vector2(element.labelName.Measure(Scale).X / 2, 0)
                        If control.icon IsNot Nothing Then
                            Dim delta As Vector2 = control.icon.Position * (Vector2.One - Scale)
                            control.icon.Draw(spriteBatch, translate - delta, scaleDelta * Scale, angleDelta + Angle, LayerDetph + layerDepthDelta)
                        End If
                        If control.iconLabel IsNot Nothing Then
                            Dim delta As Vector2 = control.iconLabel.Position * (Vector2.One - Scale)
                            control.iconLabel.Draw(spriteBatch, translate - delta, scaleDelta * Scale, angleDelta + Angle, LayerDetph + layerDepthDelta + 100)
                        End If
                    End If
                Next
                newPosition += New Vector2(0, element.labelName.Measure(Scale).Y + 1)
            End If
        Next
    End Sub

    Public Function GetLength() As Single
        Return GetLength(Vector2.One)
    End Function

    Public Function GetLength(scaleDelta As Vector2) As Single
        Dim output As Single
        For Each element In list
            If element.labelName.DrawEnable Then
                output += element.labelName.Measure(Scale * scaleDelta).Y + 1
            End If
        Next
        Return output
    End Function
End Class
