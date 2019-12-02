Imports Microsoft.Xna.Framework
Imports Microsoft.Xna.Framework.Graphics

''' <summary>
''' Uma capsula de Label para ser gerenciada pelo sistema de GUI
''' </summary>
Public Class GUILabel
    Inherits GUIObject

    ''' <summary>
    ''' Label gerenciada
    ''' </summary>
    ''' <returns> Label </returns>
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
    ''' Construtor de GUIObject
    ''' </summary>
    ''' <param name="index"> Índice</param>
    ''' <param name="indexX"> Índice X </param>
    ''' <param name="indexY"> Índice Y </param>
    ''' <param name="position"> Posição no espaço </param>
    ''' <param name="label"> Label gerenciada </param>
    Public Sub New(index As Integer, indexX As Integer, indexY As Integer, position As Vector2, label As Label)
        MyBase.New(index, indexX, indexY, position)
        Me.Label = label
        Me.GUIObjectEnable = False
    End Sub

    Public Overrides Sub Draw(ByRef spriteBatch As SpriteBatch, positionDelta As Vector2, scaleDelta As Vector2, angleDelta As Single, layerDepthDelta As UShort)
        If Label.DrawEnable Then
            Label.Draw(spriteBatch, Position + positionDelta, Scale * scaleDelta, Angle + angleDelta, LayerDetph + layerDepthDelta)
        End If
    End Sub

    ''' <summary>
    ''' Mede o tamanho do texto rederizado na tela
    ''' </summary>
    ''' <returns> Tamanho do texto desenhado </returns>
    Public Function Measure() As Vector2
        Return Measure(Me.Scale)
    End Function

    ''' <summary>
    ''' Mede o tamanho do texto rederizado na tela
    ''' </summary>
    ''' <param name="scaleDelta"> Escala adicional </param>
    ''' <returns> Tamanho do texto desenhado </returns>
    Public Function Measure(scaleDelta As Vector2) As Vector2
        Return Label.Measure(Me.Scale * scaleDelta)
    End Function


    ''' <summary>
    ''' Mede o tamanho do texto rederizado na tela a parti de um determinado caractere e com distancia determinada.
    ''' </summary>
    ''' <param name="startIndex"> Caractere inicial </param>
    ''' <param name="size"> Quantidade de caracteres </param>
    ''' <param name="scaleDelta"> Escala adicional </param>
    ''' <returns> Tamanho da substring desenhado </returns>
    Public Function MeasureIndex(startIndex As UInteger, size As Single, scaleDelta As Vector2) As Vector2
        Return Label.MeasureIndex(startIndex, size, scaleDelta * Scale)
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
End Class
