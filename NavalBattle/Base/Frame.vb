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
    ''' Cria novo quadro
    ''' </summary>
    ''' <param name="texture"> Textura </param>
    ''' <param name="origin"> Ponto que representa o centro para rotação. </param>
    ''' <param name="source"> Origem do recorte na textura(Retangulo) </param>
    ''' <param name="tint"> Tinta para desenhar(Branco/Color.White para cor completa) </param>
    ''' <param name="effects"> Efeitos, como refletir horizontalmente(SpriteEffects.FlipHorizontally) ou nada(SpriteEffects.None) </param>
    Public Sub New(texture As Texture2D, origin As Vector2, source As Rectangle, tint As Color, effects As SpriteEffects)
        Me.texture = texture
        Me.origin = origin
        Me.source = source
        Me.tint = tint
        Me.effects = effects
    End Sub
End Structure
