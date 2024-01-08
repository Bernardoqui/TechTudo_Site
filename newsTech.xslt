<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="html" indent="yes"/>

	<xsl:template match="/rss/channel">
		<html>
			<head>
				<!-- Links para o Bootstrap (já incluídos acima) -->
				<link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.0/css/bootstrap.min.css" />
				<script src="https://code.jquery.com/jquery-3.5.1.slim.min.js"></script>
				<script src="https://cdn.jsdelivr.net/npm/@popperjs/core@2.9.2/dist/umd/popper.min.js"></script>
				<script src="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.0/js/bootstrap.min.js"></script>

				<!-- Seu estilo personalizado (já incluído no seu código original) -->
				<style>

					main{
					margin-top: 10%;
					}

					.news-container {
					display: flex;
					flex-wrap: wrap;
					flex-direction: row-reverse;
					justify-content: space-around;
					align-items: stretch;
					}

					.news-item {
					width: 30%; /* Ajuste a largura conforme necessário */
					margin-bottom: 30px;
					display: flex;
					flex-direction: column;
					align-items: center; /* Alinhar elementos ao centro */
					background: #1d1d1d5e;
					border-radius: 17px;
					width: 43%;
					}

					.news-title {
					font-size: 28px;
					color: #076da2;
					text-align: center; /* Alinhar o texto ao centro */
					}

					.news-link {
					color: white;
					text-decoration: none;
					}

					.news-date {
					color: #888;
					font-size: 12px;
					font-size: 28px;
					}

					.news-image {
					max-width: 100%;
					height: auto;
					}

				</style>
			</head>
			<body>
				<div class="container" style="max-width: 100%;">
					<div class="news-container">
						<xsl:apply-templates select="item[position() &lt;= 10]" />
					</div>
				</div>
			</body>
		</html>
	</xsl:template>

	<xsl:template match="item">
		<div class="news-item">
			<a href="{link}"><img class="news-image" src="{enclosure/@url}" alt="Imagem da Notícia" /></a> 
			<h3 class="news-title">
				<a class="news-link" href="{link}" target="_blank">
					<xsl:value-of select="title" disable-output-escaping="yes"/>
				</a>
			</h3>
			<p class="news-date">
				<xsl:value-of select="pubDate"/>
			</p>
		</div>
	</xsl:template>
</xsl:stylesheet>
