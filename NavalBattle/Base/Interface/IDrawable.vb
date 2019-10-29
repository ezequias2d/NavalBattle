Imports Microsoft.Xna.Framework
Imports Microsoft.Xna.Framework.Graphics
Imports NavalBattle

#Region "FILE DESCRIPTION"
''-----------------------------------------------------------------------------
'' IDrawable.vb
''
'' Author: Ezequias Moises dos Santos Silva.
''-----------------------------------------------------------------------------
#End Region

''' <summary>
''' Interface para algo desenhavel pela Camera
''' </summary>
Public Interface IDrawable
    ''' <summary>
    ''' Determina se está ativado para desenhar.
    ''' Se verdadeiro: desenha
    ''' Se falso: não desenha
    ''' </summary>
    ''' <returns> Se ativado o desenho </returns>
    Property DrawEnable As Boolean

    ''' <summary>
    ''' Camada de desenho que pertence.
    ''' Camadas podem ser ativadas ou desativadas da redenrização de uma camera especifica.
    ''' </summary>
    ''' <returns> Numero da camada </returns>
    Property Layer As Long

    ''' <summary>
    ''' Camada de sobreposição
    ''' </summary>
    ''' <returns> Camada que pertence </returns>
    Property LayerDetph As UShort

    ''' <summary>
    ''' Função de desenho
    ''' </summary>
    ''' <param name="spriteBatch"> SpriteBatch para desenhar </param>
    Sub Draw(ByRef spriteBatch As SpriteBatch)

    ''' <summary>
    ''' Função de desenho
    ''' </summary>
    ''' <param name="spriteBatch"> SpriteBatch para desenhar </param>
    ''' <param name="layerDepthDelta"> Camada adicional </param>
    Sub Draw(ByRef spriteBatch As SpriteBatch, layerDepthDelta As UShort)

    ''' <summary>
    ''' Função de desenho
    ''' </summary>
    ''' <param name="spriteBatch"> SpriteBatch para desenhar </param>
    ''' <param name="layerDepthDelta"> Camada adicional </param>
    ''' <param name="transformDelta"> Transform adicional </param>
    Sub Draw(ByRef spriteBatch As SpriteBatch, transformDelta As ITransform, layerDepthDelta As UShort)

    ''' <summary>
    ''' Função de desenho
    ''' </summary>
    ''' <param name="spriteBatch"> SpriteBatch para desenhar </param>
    ''' <param name="positionDelta"> Posição adicional </param>
    ''' <param name="scaleDelta"> Escala adicional </param>
    ''' <param name="angleDelta"> Ângulo adicional </param>
    ''' <param name="layerDepthDelta"> Camada adicional </param>
    Sub Draw(ByRef spriteBatch As SpriteBatch, positionDelta As Vector2, scaleDelta As Vector2, angleDelta As Single, layerDepthDelta As UShort)

End Interface
