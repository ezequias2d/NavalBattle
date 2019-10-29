Imports Microsoft.Xna.Framework
Imports Microsoft.Xna.Framework.Graphics

#Region "FILE DESCRIPTION"
''-----------------------------------------------------------------------------
'' Frame.vb
''
'' Author: Ezequias Moises dos Santos Silva.
''-----------------------------------------------------------------------------
#End Region

''' <summary>
''' Pedaço de um Texture2D
''' </summary>
Public Structure Frame
    ''' <summary>
    ''' Textura
    ''' </summary>
    Public texture As Texture2D

    ''' <summary>
    ''' Origem para rotação
    ''' </summary>
    Public origin As Vector2

    ''' <summary>
    ''' Retangulo de corte
    ''' </summary>
    Public source As Rectangle

    ''' <summary>
    ''' Cor de desenho, Cor Branco(<see cref="Color.White"/>) para cores completas.
    ''' </summary>
    Public tint As Color

    ''' <summary>
    ''' Efeitos(<see cref ="SpriteEffects"/>) de modificação
    ''' </summary>
    Public effects As SpriteEffects

    ''' <summary>
    ''' Flag que determina se o sprite sera esticado pela escala(false) ou repetido(true)
    ''' </summary>
    Public infinity As Boolean

    ''' <summary>
    ''' Cria novo quadro
    ''' </summary>
    ''' <param name="texture"> Textura </param>
    ''' <param name="origin"> Ponto que representa o centro para rotação. </param>
    ''' <param name="source"> Origem do recorte na textura(Retangulo) </param>
    ''' <param name="tint"> Tinta para desenhar(Branco/Color.White para cor completa) </param>
    ''' <param name="effects"> Efeitos, como refletir horizontalmente(SpriteEffects.FlipHorizontally) ou nada(SpriteEffects.None) </param>
    Public Sub New(texture As Texture2D, origin As Vector2, source As Rectangle, tint As Color, effects As SpriteEffects)
        Me.New(texture, origin, source, tint, effects, False)
    End Sub

    Public Sub New(texture As Texture2D, source As Rectangle)
        Me.New(texture, Vector2.Zero, source, Color.White, SpriteEffects.None)
    End Sub

    Public Sub New(texture As Texture2D, source As Rectangle, effects As SpriteEffects)
        Me.New(texture, Vector2.Zero, source, Color.White, effects)
    End Sub

    Public Sub New(texture As Texture2D, source As Rectangle, infinity As Boolean)
        Me.New(texture, Vector2.Zero, source, Color.White, SpriteEffects.None, infinity)
    End Sub

    ''' <summary>
    ''' Cria novo quadro
    ''' </summary>
    ''' <param name="texture"> Textura </param>
    ''' <param name="origin"> Ponto que representa o centro para rotação. </param>
    ''' <param name="source"> Origem do recorte na textura(Retangulo) </param>
    ''' <param name="tint"> Tinta para desenhar(Branco/Color.White para cor completa) </param>
    ''' <param name="effects"> Efeitos, como refletir horizontalmente(SpriteEffects.FlipHorizontally) ou nada(SpriteEffects.None) </param>
    ''' <param name="infinity"> Se a imagem se repete em vez de esticar </param>
    Public Sub New(texture As Texture2D, origin As Vector2, source As Rectangle, tint As Color, effects As SpriteEffects, infinity As Boolean)
        Me.texture = texture
        Me.origin = origin
        Me.source = source
        Me.tint = tint
        Me.effects = effects
        Me.infinity = infinity
    End Sub

    ''' <summary>
    ''' Desenha usando transform como posição, escala e ângulo.
    ''' </summary>
    ''' <param name="spriteBatch"> SpriteBatch </param>
    ''' <param name="transform"> Transform</param>
    ''' <param name="layerDepth"> LayerDepth </param>
    Public Sub Draw(ByRef spriteBatch As SpriteBatch, ByVal transform As ITransform, ByVal layerDepth As UShort)
        Me.Draw(spriteBatch, transform.Position, transform.Scale, transform.Angle, layerDepth)
    End Sub

    ''' <summary>
    ''' Desenha usando uma posição, escala e ângulo especifico.
    ''' </summary>
    ''' <param name="spriteBatch"> SpriteBatch </param>
    ''' <param name="position"> Position </param>
    ''' <param name="scale"> Scale </param>
    ''' <param name="angle"> Angle</param>
    ''' <param name="layerDepth"> LayerDepth </param>
    Public Sub Draw(ByRef spriteBatch As SpriteBatch, ByVal position As Vector2, ByVal scale As Vector2, ByVal angle As Single, ByVal layerDepth As UShort)
        If texture Is Nothing Then
            Return
        End If
        If Not infinity Then

            spriteBatch.Draw(texture,
                         position,
                         source,
                         tint,
                         MathHelper.ToRadians(angle),
                         origin,
                         scale,
                         effects,
                         layerDepth / 65536.0F)
        Else
            'Repete a textura em vez de esticar.
            Dim matrix As Matrix = Matrix.CreateRotationZ(MathHelper.ToRadians(angle))
            Dim size As Vector2 = New Vector2(source.Width * scale.X, source.Height * scale.Y)
            Dim origin As Vector2 = origin * scale

            Dim IMax As Integer = scale.X - 1 - (scale.X Mod 1.0F)
            Dim JMax As Integer = scale.Y - 1 - (scale.Y Mod 1.0F)

            For i As Integer = 0 To IMax
                For j As Integer = 0 To JMax
                    spriteBatch.Draw(texture,
                        position + Vector2.Transform(New Vector2(i * source.Width, j * source.Height), matrix),
                        source,
                        tint,
                        MathHelper.ToRadians(angle),
                        origin,
                        New Vector2(1.0F, 1.0F),
                        effects,
                        layerDepth / 65536.0F)
                Next
            Next

            Dim restRight As Rectangle = source
            restRight.Width *= scale.X Mod 1.0F

            Dim restDown As Rectangle = source
            restDown.Height *= scale.Y Mod 1.0F

            Dim restRightDown As Rectangle = source
            restRightDown.Width *= scale.X Mod 1.0F
            restRightDown.Height *= scale.Y Mod 1.0F

            For j As Integer = 0 To JMax
                spriteBatch.Draw(texture,
                        position + Vector2.Transform(New Vector2((IMax + 1) * source.Width, j * source.Height), matrix),
                        restRight,
                        tint,
                        MathHelper.ToRadians(angle),
                        origin,
                        New Vector2(1.0F, 1.0F),
                        effects,
                        layerDepth / 65536.0F)
            Next

            For i As Integer = 0 To IMax
                spriteBatch.Draw(texture,
                        position + Vector2.Transform(New Vector2(i * source.Width, (JMax + 1) * source.Height), matrix),
                        restDown,
                        tint,
                        MathHelper.ToRadians(angle),
                        origin,
                        New Vector2(1.0F, 1.0F),
                        effects,
                        layerDepth / 65536.0F)
            Next

            spriteBatch.Draw(texture,
                        position + Vector2.Transform(New Vector2((IMax + 1) * source.Width, (JMax + 1) * source.Height), matrix),
                        restRightDown,
                        tint,
                        MathHelper.ToRadians(angle),
                        origin,
                        New Vector2(1.0F, 1.0F),
                        effects,
                        layerDepth / 65536.0F)


        End If
    End Sub
End Structure
