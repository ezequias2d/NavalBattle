Imports Microsoft.Xna.Framework
Imports Microsoft.Xna.Framework.Graphics

#Region "FILE DESCRIPTION"
''-----------------------------------------------------------------------------
'' Button.vb
''
'' Author: Ezequias Moises dos Santos Silva.
''-----------------------------------------------------------------------------
#End Region

''' <summary>
''' Botão que se adapta ao texto
''' </summary>
Public Class Button
    Inherits GUIObject

    Public Property Box As Box
    Public Property BoxSelected As Box
    Public Property Label As Label

    Public Property GetStringFunction As Label.GetString
        Get
            Return Label.GetStringFunction
        End Get
        Set(value As Label.GetString)
            Label.GetStringFunction = value
        End Set
    End Property

    ''' <summary>
    ''' Construtor de botão
    ''' </summary>
    ''' <param name="index"> Índice </param>
    ''' <param name="indexX"> Índice X </param>
    ''' <param name="indexY"> Índece Y </param>
    ''' <param name="position"> Posição espacial </param>
    ''' <param name="text"> Texto </param>
    ''' <param name="scale"> Escala do texto </param>
    Public Sub New(index As Integer, indexX As Integer, indexY As Integer, position As Vector2, text As String, scale As Vector2)
        MyBase.New(index, indexX, indexY, position)
        Me.Scale = scale
        Me.CursorMode = CursorMode.Clove
        Me.CursorColor = AddressOf Button.GetColor

        Label = New Label(text, Color.Black, Label.Font)
        Label.LayerDetph = 2
        Dim textSize As Vector2 = Label.Measure(Vector2.One)
        textSize.X = Math.Max(textSize.X + 12.0F, 16.0F)
        textSize.Y = Math.Max(textSize.Y + 12.0F, 16.0F)

        Box = New Box(textSize, New Frame(GUIController.Texture, New Rectangle(0, 0, 24, 24)))
        BoxSelected = New Box(textSize, New Frame(GUIController.Texture, New Rectangle(24, 0, 24, 24)))
        BoxSelected.LayerDepth = 1
        Box.LayerDepth = 1

        Origin = -Vector2.One * 1.5F

        AxisModeSetting(Axis.Fire0) = AxisMode.Click
        AxisModeSetting(Axis.Fire1) = AxisMode.Click
        AxisModeSetting(Axis.Fire2) = AxisMode.Click
        AxisModeSetting(Axis.Jump) = AxisMode.Click
    End Sub

    ''' <summary>
    ''' Construtor de botão
    ''' </summary>
    ''' <param name="index"> Índece </param>
    ''' <param name="indexX"> Índece X </param>
    ''' <param name="indexY"> Índice Y </param>
    ''' <param name="position"> Posição espacial </param>
    ''' <param name="frameSelected"> Frame de seleção </param>
    Public Sub New(index As Integer, indexX As Integer, indexY As Integer, position As Vector2, frameSelected As Frame, size As Vector2)
        MyBase.New(index, indexX, indexY, position)
        Me.CursorMode = CursorMode.Clove
        Me.CursorColor = AddressOf Button.GetColor

        Me.Origin = New Vector2(frameSelected.source.Width, frameSelected.source.Height) / 2.0F

        BoxSelected = New Box(size, frameSelected)
        BoxSelected.LayerDepth = 1
        AxisModeSetting(Axis.Fire0) = AxisMode.Click
        AxisModeSetting(Axis.Fire1) = AxisMode.Click
        AxisModeSetting(Axis.Fire2) = AxisMode.Click
        AxisModeSetting(Axis.Jump) = AxisMode.Click
    End Sub

    ''' <summary>
    ''' Retorna a cor do botão, dependendo do tipo de ação possivel.
    ''' </summary>
    ''' <param name="obj"></param>
    ''' <returns></returns>
    Private Shared Function GetColor(ByVal obj As GUIObject) As Color
        Dim fire0 As Boolean = obj.OnFire0 IsNot Nothing
        Dim fire1 As Boolean = obj.OnFire1 IsNot Nothing
        Dim fire2 As Boolean = obj.OnFire2 IsNot Nothing
        Dim submit As Boolean = obj.OnSubmit IsNot Nothing

        Dim colorFire0 As Color = Color.GreenYellow
        Dim colorFire1 As Color = Color.OrangeRed
        Dim colorFire2 As Color = Color.RoyalBlue
        Dim colorSubmit As Color = Color.ForestGreen

        Dim colorOut As Color = Color.MediumVioletRed
        If fire0 OrElse fire1 OrElse fire2 OrElse submit Then
            colorOut.R = (0L + colorFire0.R * fire0 + colorFire1.R * fire1 + colorFire2.R * fire2 + colorSubmit.R * submit) / (0L + fire0 + fire1 + fire2 + submit)
            colorOut.G = (0L + colorFire0.G * fire0 + colorFire1.G * fire1 + colorFire2.G * fire2 + colorSubmit.G * submit) / (0L + fire0 + fire1 + fire2 + submit)
            colorOut.B = (0L + colorFire0.B * fire0 + colorFire1.B * fire1 + colorFire2.B * fire2 + colorSubmit.B * submit) / (0L + fire0 + fire1 + fire2 + submit)
        End If

        Return colorOut
    End Function

    ''' <summary>
    ''' Desenha botão
    ''' </summary>
    ''' <param name="spriteBatch"> SpriteBatch </param>
    ''' <param name="positionDelta"> Posição adicional </param>
    ''' <param name="scaleDelta"> Escala adicional </param>
    ''' <param name="angleDelta"> Ângulo andicional</param>
    ''' <param name="layerDepthDelta"> Camada de profundidade de sprite </param>
    Public Overrides Sub Draw(ByRef spriteBatch As SpriteBatch, positionDelta As Vector2, scaleDelta As Vector2, angleDelta As Single, layerDepthDelta As UShort)
        Dim pos As Vector2 = PositionTranslated + positionDelta + Origin * Scale * scaleDelta

        If Label IsNot Nothing Then
            Dim textSize As Vector2 = Label.Measure()
            textSize.X = Math.Max(textSize.X + 12.0F, 16.0F)
            textSize.Y = Math.Max(textSize.Y + 12.0F, 16.0F)
            BoxSelected.Size = textSize
            Box.Size = textSize
        End If

        If Label IsNot Nothing AndAlso Label.DrawEnable Then
            Label.Draw(spriteBatch, pos, Scale * scaleDelta, Angle + angleDelta, layerDepthDelta + LayerDetph)
        End If

        If Selected Then
            If BoxSelected IsNot Nothing AndAlso BoxSelected.DrawEnable Then
                BoxSelected.Draw(spriteBatch, pos, Scale * scaleDelta, Angle + angleDelta, layerDepthDelta + LayerDetph)
            End If
        Else
            If Box IsNot Nothing AndAlso Box.DrawEnable Then
                Box.Draw(spriteBatch, pos, Scale * scaleDelta, Angle + angleDelta, layerDepthDelta + LayerDetph)
            End If
        End If
    End Sub

End Class

