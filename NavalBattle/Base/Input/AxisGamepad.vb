
Imports Microsoft.Xna.Framework
Imports Microsoft.Xna.Framework.Input
Imports NavalBattle

#Region "FILE DESCRIPTION"
''-----------------------------------------------------------------------------
'' AxisGamePad.vb
''
'' Author: Ezequias Moises dos Santos Silva.
''-----------------------------------------------------------------------------
#End Region

''' <summary>
''' AxisGamepad
''' Eixo que varia de -1.0F a 1.0F utilizando como entrada um Gamepad
''' </summary>
Public Class AxisGamepad
    Implements IAxis

    ''' <summary>
    ''' Construtor de AxisGamepad
    ''' </summary>
    ''' <param name="axis"> Axis associado </param>
    ''' <param name="positive"> Botão que altera positivamente </param>
    ''' <param name="negative"> Botão que altera negativamente </param>
    Public Sub New(axis As Axis, positive As GamePadButton, negative As GamePadButton)
        Me.Axis = axis
        Me.Positive = positive
        Me.Negative = negative
    End Sub

    ''' <summary>
    ''' Axis associado
    ''' </summary>
    ''' <returns> Axis </returns>
    Public Property Axis As Axis Implements IAxis.Axis

    ''' <summary>
    ''' Botão que altera positivamente
    ''' </summary>
    ''' <returns> Botão associado positivamente </returns>
    Public Property Positive As GamePadButton

    ''' <summary>
    ''' Botão que altera negativamente
    ''' </summary>
    ''' <returns> Botão associado negativamente </returns>
    Public Property Negative As GamePadButton

    ''' <summary>
    ''' Calcula o valor do Axis.
    ''' </summary>
    ''' <returns> Valor do Axis. </returns>
    Public Function GetValue() As Single Implements IAxis.GetValue
        Dim gampadState As GamePadState = GamePad.GetState(PlayerIndex.One)
        Dim output As Single = 0

        Select Case Positive
            Case GamePadButton.A
                If gampadState.Buttons.A = ButtonState.Pressed Then
                    output += 1
                End If
            Case GamePadButton.B
                If gampadState.Buttons.B = ButtonState.Pressed Then
                    output += 1
                End If
            Case GamePadButton.X
                If gampadState.Buttons.X = ButtonState.Pressed Then
                    output += 1
                End If
            Case GamePadButton.Y
                If gampadState.Buttons.Y = ButtonState.Pressed Then
                    output += 1
                End If

            Case GamePadButton.Start
                If gampadState.Buttons.Start = ButtonState.Pressed Then
                    output += 1
                End If

            Case GamePadButton.Back
                If gampadState.Buttons.Back = ButtonState.Pressed Then
                    output += 1
                End If

            Case GamePadButton.LeftShoulder
                If gampadState.Buttons.LeftShoulder = ButtonState.Pressed Then
                    output += 1
                End If

            Case GamePadButton.RightShoulder
                If gampadState.Buttons.RightShoulder = ButtonState.Pressed Then
                    output += 1
                End If

            Case GamePadButton.LeftShoulder
                If gampadState.Buttons.LeftShoulder = ButtonState.Pressed Then
                    output += 1
                End If

            Case GamePadButton.XAxisLeft
                output += gampadState.ThumbSticks.Left.X

            Case GamePadButton.YAxisLeft
                output += gampadState.ThumbSticks.Left.Y

            Case GamePadButton.XAxisRight
                output += gampadState.ThumbSticks.Right.X

            Case GamePadButton.YAxisRight
                output += gampadState.ThumbSticks.Right.Y

            Case GamePadButton.DPadX
                If gampadState.DPad.Right = ButtonState.Pressed Then
                    output += 1.0F
                End If

            Case GamePadButton.DPadY
                If gampadState.DPad.Up = ButtonState.Pressed Then
                    output -= 1.0F
                End If
        End Select

        Select Case Negative
            Case GamePadButton.A
                If gampadState.Buttons.A = ButtonState.Pressed Then
                    output -= 1
                End If
            Case GamePadButton.B
                If gampadState.Buttons.B = ButtonState.Pressed Then
                    output -= 1
                End If
            Case GamePadButton.X
                If gampadState.Buttons.X = ButtonState.Pressed Then
                    output -= 1
                End If
            Case GamePadButton.Y
                If gampadState.Buttons.Y = ButtonState.Pressed Then
                    output -= 1
                End If

            Case GamePadButton.Start
                If gampadState.Buttons.Start = ButtonState.Pressed Then
                    output -= 1
                End If

            Case GamePadButton.Back
                If gampadState.Buttons.Back = ButtonState.Pressed Then
                    output -= 1
                End If

            Case GamePadButton.LeftShoulder
                If gampadState.Buttons.LeftShoulder = ButtonState.Pressed Then
                    output -= 1
                End If

            Case GamePadButton.RightShoulder
                If gampadState.Buttons.RightShoulder = ButtonState.Pressed Then
                    output -= 1
                End If

            Case GamePadButton.LeftShoulder
                If gampadState.Buttons.LeftShoulder = ButtonState.Pressed Then
                    output -= 1
                End If

            Case GamePadButton.XAxisLeft
                output -= gampadState.ThumbSticks.Left.X

            Case GamePadButton.YAxisLeft
                output -= gampadState.ThumbSticks.Left.Y

            Case GamePadButton.XAxisRight
                output -= gampadState.ThumbSticks.Right.X

            Case GamePadButton.YAxisRight
                output -= gampadState.ThumbSticks.Right.Y

            Case GamePadButton.DPadX
                If gampadState.DPad.Left = ButtonState.Pressed Then
                    output -= 1.0F
                End If

            Case GamePadButton.DPadY
                If gampadState.DPad.Down = ButtonState.Pressed Then
                    output += 1.0F
                End If
        End Select

        Return output
    End Function
End Class
