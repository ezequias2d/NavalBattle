Imports Microsoft.Xna.Framework
Imports Microsoft.Xna.Framework.Graphics
Imports NavalBattle

#Region "FILE DESCRIPTION"
''-----------------------------------------------------------------------------
'' Label.vb
''
'' Author: Ezequias Moises dos Santos Silva.
''-----------------------------------------------------------------------------
#End Region

''' <summary>
''' Rotulo
''' Desenha um texto(Text) com a fonte(SpriteFont) na posição e com cor(Color)
''' </summary>
Public Class Label
    Implements IDrawable
    Implements ITransform

    Public Shared ReadOnly FontName As String = "fonts/GrrMono0.1"
    Private Shared _Font As SpriteFont

    Public Shared ReadOnly Property Font As SpriteFont
        Get
            If _Font Is Nothing Then
                _Font = ScreenManager.Instance.Content.Load(Of SpriteFont)(FontName)
            End If
            Return _Font
        End Get
    End Property

    Public Sub New(text As String, color As Color, spriteFont As SpriteFont)
        Me.Text = text
        Me.Color = color
        Me.SpriteFont = spriteFont

        Me.DrawEnable = True
        Me.AlignmentX = Alignment.Center
        Me.AlignmentY = Alignment.Center
        Me.Position = Vector2.Zero
        Me.Angle = 0F
        Me.Scale = Vector2.One
    End Sub

    ''' <summary>
    ''' Determina se está ativado para desenhar.
    ''' Se verdadeiro: desenha
    ''' Se falso: não desenha
    ''' </summary>
    ''' <returns> Se ativado o desenho </returns>
    Public Property DrawEnable As Boolean Implements IDrawable.DrawEnable

    ''' <summary>
    ''' Camada de desenho que pertence.
    ''' Camadas podem ser ativadas ou desativadas da redenrização de uma camera especifica.
    ''' </summary>
    ''' <returns> Numero da camada </returns>
    Public Property Layer As Long Implements IDrawable.Layer

    ''' <summary>
    ''' Posição da Label
    ''' </summary>
    Public Property Position As Vector2 Implements ITransform.Position

    ''' <summary>
    ''' Angulo de rotação sobre o eixo Z
    ''' </summary>
    ''' <returns></returns>
    Public Property Angle As Single Implements ITransform.Angle

    ''' <summary>
    ''' Escala no espaço virtual
    ''' </summary>
    ''' <returns> Escala no espaço virtual </returns>
    Public Property Scale As Vector2 Implements ITransform.Scale

    ''' <summary>
    ''' Texto desenhado pelo Label
    ''' </summary>
    ''' <returns> Texto </returns>
    Public Property Text As String

    ''' <summary>
    ''' Cor do texto desenhado
    ''' </summary>
    ''' <returns> Cor </returns>
    Public Property Color As Color

    ''' <summary>
    ''' Camada de sobreposição
    ''' </summary>
    ''' <returns> Camada de sobreposição </returns>
    Public Property LayerDetph As UShort Implements IDrawable.LayerDetph

    ''' <summary>
    ''' Fonte do texto
    ''' </summary>
    ''' <returns> Fonte </returns>
    Public Property SpriteFont As SpriteFont

    ''' <summary>
    ''' Alinhamento do eixo X entre o texto e a posição
    ''' </summary>
    ''' <returns> Alinhamento em X</returns>
    Public Property AlignmentX As Alignment

    ''' <summary>
    ''' Alinhamento do eixo Y entre o texto e a posição
    ''' </summary>
    ''' <returns> Alinhamento em X</returns>
    Public Property AlignmentY As Alignment

    ''' <summary>
    ''' Calcula deslocamento do texto para alinhar ao eixo X e Y
    ''' </summary>
    ''' <returns> Variação de posição necessaria para alinhar </returns>
    Private Function CalculateOrigin() As Vector2
        Dim origin As Vector2 = Vector2.Zero
        Dim size As Vector2 = SpriteFont.MeasureString(Text)

        Select Case AlignmentX
            Case Alignment.Center
                origin.X += size.X / 2.0F
            Case Alignment.Ending
                origin.X += size.X
        End Select

        Select Case AlignmentY
            Case Alignment.Center
                origin.Y += size.Y / 2.0F
            Case Alignment.Ending
                origin.Y += size.Y
        End Select

        Return origin
    End Function

    ''' <summary>
    ''' Desenha texto
    ''' </summary>
    ''' <param name="spriteBatch"></param>
    Public Sub Draw(ByRef spriteBatch As SpriteBatch) Implements IDrawable.Draw
        Draw(spriteBatch, Vector2.Zero, Vector2.One, 0F, 0US)
    End Sub

    ''' <summary>
    ''' Desenha texto
    ''' </summary>
    ''' <param name="spriteBatch"> SpriteBatch </param>
    ''' <param name="layerDepthDelta"> LayerDepth adicional </param>
    Public Sub Draw(ByRef spriteBatch As SpriteBatch, layerDepthDelta As UShort) Implements IDrawable.Draw
        Draw(spriteBatch, Vector2.Zero, Vector2.One, 0F, layerDepthDelta)
    End Sub

    ''' <summary>
    ''' Desenha texto
    ''' </summary>
    ''' <param name="spriteBatch"> SpriteBatch </param>
    ''' <param name="transformDelta"> Transform adicional </param>
    ''' <param name="layerDepthDelta"> LayerDepth adicional </param>
    Public Sub Draw(ByRef spriteBatch As SpriteBatch, transformDelta As ITransform, layerDepthDelta As UShort) Implements IDrawable.Draw
        Draw(spriteBatch, transformDelta.Position, transformDelta.Scale, transformDelta.Angle, layerDepthDelta)
    End Sub

    ''' <summary>
    ''' Desenha texto
    ''' </summary>
    ''' <param name="spriteBatch"> SpriteBatch </param>
    ''' <param name="positionDelta"> Posição adicional </param>
    ''' <param name="scaleDelta"> Escala adicional </param>
    ''' <param name="angleDelta"> Angulo adicional </param>
    ''' <param name="layerDepthDelta"> LayerDepth adicional </param>
    Public Sub Draw(ByRef spriteBatch As SpriteBatch, positionDelta As Vector2, scaleDelta As Vector2, angleDelta As Single, layerDepthDelta As UShort) Implements IDrawable.Draw
        Dim origin As Vector2 = CalculateOrigin()
        spriteBatch.DrawString(SpriteFont, Text, Position + positionDelta, Color, MathHelper.ToRadians(Angle + angleDelta), origin, Scale * scaleDelta, SpriteEffects.None, (LayerDetph + layerDepthDelta) / 65536.0F)
    End Sub

    ''' <summary>
    ''' Mede o tamanho do texto rederizado na tela
    ''' </summary>
    ''' <param name="scaleDelta"> Escala adicional </param>
    ''' <returns> Tamanho do texto desenhado </returns>
    Public Function Measure(scaleDelta As Vector2) As Vector2
        Return SpriteFont.MeasureString(Text) * scaleDelta * Scale
    End Function

    ''' <summary>
    ''' Mede o tamanho do texto rederizado na tela
    ''' </summary>
    ''' <returns> Tamanho do texto desenhado </returns>
    Public Function Measure() As Vector2
        Return SpriteFont.MeasureString(Text) * Scale
    End Function

    ''' <summary>
    '''  Mede o tamanho do texto rederizado na tela a parti de um determinado caractere e com distancia determinada.
    ''' </summary>
    ''' <param name="startIndex"> Caractere inicial </param>
    ''' <param name="size"> Quantidade de caracteres </param>
    ''' <returns> Tamanho da substring desenhado </returns>
    Public Function MeasureIndex(startIndex As UInteger, size As Single) As Vector2
        Return MeasureIndex(startIndex, size, Vector2.One)
    End Function

    ''' <summary>
    ''' Mede o tamanho do texto rederizado na tela a parti de um determinado caractere e com distancia determinada.
    ''' </summary>
    ''' <param name="startIndex"> Caractere inicial </param>
    ''' <param name="size"> Quantidade de caracteres </param>
    ''' <param name="scaleDelta"> Escala adicional </param>
    ''' <returns> Tamanho da substring desenhado </returns>
    Public Function MeasureIndex(startIndex As UInteger, size As Single, scaleDelta As Vector2) As Vector2
        Dim sizeInt As Integer = size - (size Mod 1)
        Dim output As Vector2 = SpriteFont.MeasureString(Text.Substring(startIndex, sizeInt)) * Scale * scaleDelta

        output += Vector2.UnitX * SpriteFont.MeasureString(Text.Substring(startIndex + sizeInt, 1)).X * Scale * scaleDelta * (size Mod 1)

        Return output
    End Function
End Class
