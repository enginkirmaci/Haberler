namespace Haberler.Services.Constants
{
    public class HtmlConstants
    {
        static readonly public string HTML =
@"<html>
    <head>
        <meta http-equiv='Content-Type' content='text/html;charset=UTF-8'>
        <meta name='viewport' content='width=device-width, initial-scale=1.0'>
        <title>{1}</title>
        <style>
        {0}
        </style>
        {9}
    </head>
<body style='font-size: {8}px;'>
    <article>
        <header>
            <h1 id='page-title'>{1}</h1>
            <h2 id='page-subtitle'><a href='{2}'>{3}</a>, {4}</h2>
        </header>
        <img src='{5}' alt='{1}' {6} />
        {7}
    </article>
    <br/>
    <br/>
</body>
</html>";

        static readonly public string JS =
@"<script>
    var fontSize = 1;

    function ChangeFontSize(value) {
        fontSize = value;
        document.body.style.fontSize = value.toString() + 'px';
        window.external.notify('The script says the doubled value is ' + value.toString());
    }

    function scrollVertical(value) {
        window.scrollTo(window.pageXOffset, (window.pageYOffset + parseFloat(value)));
        window.external.notify( (window.pageYOffset + parseFloat(value)).toString() );
    };
</script>";

        static readonly public string BaseTheme =
@"      @media all and (orientation:portrait) {
            @-ms-viewport {
                user-zoom: fixed;
                max-zoom: 1;
                min-zoom: 1;
            }
        }

        @media all and (orientation:landscape) {
            @-ms-viewport {
                user-zoom: fixed;
                max-zoom: 1;
                min-zoom: 1;
            }
        }

        html, body {
            height: 100%;
            width: 100%;
            margin: 0px;
            padding: 0px;
        }

        body {
            font-family: 'Segoe WP';
            letter-spacing: 0.02em;
        }

        blockquote {
            margin: 0;
        }

        @media only screen and (min-width: 960px) {
            body {
                width: 960px;
                margin: 0 auto;
            }
        }

        article {
            padding:12px;
        }

        article article {
            padding:0px;
        }

        h2
        {
            font-size: 125%;
        }

        #page-title, #page-title a {
            font-family: 'Segoe WP';
            font-size: 150%;
            margin: 0 0 -10px 0;
            font-weight: bold; 
        }

        #page-subtitle, #page-subtitle a {
            font-family: 'Segoe WP SemiLight';
            font-size: 95%;
            letter-spacing: 0.04em;
            font-weight: bold;
        }

        #page-subtitle a {
            border-bottom: dotted 2px;
            text-decoration: none;
        }

        a:link, a:active, a:visited, a:hover {
            text-decoration: underline;
        }

        input { display: none }

        img { width: 100%; }
";

        static readonly public string DarkTheme =
@"      body {
            background-color: #1E1E1E;
            color: #FFFFFF;
        }

         #page-title, #page-title a {
            color: #FFFFFF;
        }

        #page-subtitle, #page-subtitle a {
            color: #EE243F;
        }

        a:link, a:active, a:visited, a:hover {
            color: #FFFFFF;
        }
    ";

        static readonly public string LightTheme =
@"      body {
            background-color: #FFFFFF;
            color: #1E1E1E;
        }

         #page-title, #page-title a {
            color: #1E1E1E;
        }

        #page-subtitle, #page-subtitle a {
            color: #EE243F;
        }

        a:link, a:active, a:visited, a:hover {
            color: #1E1E1E;
        }
    ";

        static readonly public string SepiaTheme =
@"      body {
            background-color: #FBF0DA;
            color: #534230;
        }

         #page-title, #page-title a {
           color: #534230;
        }

        #page-subtitle, #page-subtitle a {
            color: #EE243F;
        }

        a:link, a:active, a:visited, a:hover {
            color: #534230;
        }
    ";

        static readonly public string HideStyle = "style='display:none'";
    }
}