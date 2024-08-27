using System;
using System.IO;
using System.Net.Mime;
using System.Collections.Generic;
using System.Globalization;


namespace Curso_Basico.Helpers
{
    public class LectorFicheros
    {
        private const string EXCEPCION_FICHERO_NO_EXISTE = "No existe fichero y no se puede leer. Obvio!";

        private readonly Dictionary<string, string> _MimeMapping = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
                        { ".323", "text/h323" },
            { ".3g2", "video/3gpp2" },
            { ".3gp", "video/3gpp" },
            { ".7z", "application/x-7z-compressed" },
            { ".aac", "audio/aac" },
            { ".abw", "application/x-abiword" },
            { ".azw", "application/vnd.amazon.ebook" },
            { ".csh", "application/x-csh" },
            { ".css", "text/css" },
            { ".csv", "text/csv" },
            { ".doc", "application/msword" },
            { ".docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document" },
            { ".eot", "application/vnd.ms-fontobject" },
            { ".epub", "application/epub+zip" },
            { ".html", "text/html" },
            { ".ics", "text/calendar" },
            { ".jar", "application/java-archive" },
            { ".json", "application/json" },
            { ".jsonld", "application/ld+json" },
            { ".mjs", "text/javascript" },
            { ".mpkg", "application/vnd.apple.installer+xml" },
            { ".odp", "application/vnd.oasis.opendocument.presentation" },
            { ".ods", "application/vnd.oasis.opendocument.spreadsheet" },
            { ".odt", "application/vnd.oasis.opendocument.text" },
            { ".oga", "audio/ogg" },
            { ".ogv", "video/ogg" },
            { ".ogx", "application/ogg" },
            { ".opus", "audio/opus" },
            { ".otf", "font/otf" },
            { ".php", "application/x-httpd-php" },
            { ".rar", "application/x-rar-compressed" },
            { ".svg", "image/svg+xml" },
            { ".tif", "image/tiff" },
            { ".ts", "video/mp2t" },
            { ".ttf", "font/ttf" },
            { ".vsd", "application/vnd.visio" },
            { ".wav", "audio/wav" },
            { ".weba", "audio/webm" },
            { ".webm", "video/webm" },
            { ".webp", "image/webp" },
            { ".woff", "font/woff" },
            { ".woff2", "font/woff2" },
            { ".xhtml", "application/xhtml+xml" },
            { ".xls", "application/vnd.ms-excel" },
            { ".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" },
            { ".xml", "application/xml" },
            { ".xul", "application/vnd.mozilla.xul+xml" },
            { ".zip", "application/zip" },
            { ".3dm", "x-world/x-3dmf" },
            { ".3dmf", "x-world/x-3dmf" },
            { ".a", "application/octet-stream" },
            { ".aab", "application/x-authorware-bin" },
            { ".aam", "application/x-authorware-map" },
            { ".aas", "application/x-authorware-seg" },
            { ".abc", "text/vnd.abc" },
            { ".acgi", "text/html" },
            { ".afl", "video/animaflex" },
            { ".ai", "application/postscript" },
            { ".aif", "audio/aiff" },
            { ".aifc", "audio/aiff" },
            { ".aiff", "audio/aiff" },
            { ".aim", "application/x-aim" },
            { ".aip", "text/x-audiosoft-intra" },
            { ".ani", "application/x-navi-animation" },
            { ".aos", "application/x-nokia-9000-communicator-add-on-software" },
            { ".aps", "application/mime" },
            { ".arc", "application/octet-stream" },
            { ".arj", "application/arj" },
            { ".art", "image/x-jg" },
            { ".asf", "video/x-ms-asf" },
            { ".asm", "text/x-asm" },
            { ".asp", "text/asp" },
            { ".asx", "video/x-ms-asf" },
            { ".au", "audio/basic" },
            { ".avi", "video/avi" },
            { ".avs", "video/avs-video" },
            { ".bcpio", "application/x-bcpio" },
            { ".bin", "application/mac-binary" },
            { ".bm", "image/bmp" },
            { ".bmp", "image/bmp" },
            { ".boo", "application/book" },
            { ".book", "application/book" },
            { ".boz", "application/x-bzip2" },
            { ".bsh", "application/x-bsh" },
            { ".bz", "application/x-bzip" },
            { ".bz2", "application/x-bzip2" },
            { ".c", "text/plain" },
            { ".c++", "text/plain" },
            { ".cat", "application/vnd.ms-pki.seccat" },
            { ".cc", "text/plain" },
            { ".ccad", "application/clariscad" },
            { ".cco", "application/x-cocoa" },
            { ".cdf", "application/cdf" },
            { ".cer", "application/pkix-cert" },
            { ".cha", "application/x-chat" },
            { ".chat", "application/x-chat" },
            { ".class", "application/java" },
            { ".cpio", "application/x-cpio" },
            { ".cpp", "text/x-c" },
            { ".cpt", "application/mac-compactpro" },
            { ".crl", "application/pkcs-crl" },
            { ".crt", "application/pkix-cert" },
            { ".cxx", "text/plain" },
            { ".dcr", "application/x-director" },
            { ".deepv", "application/x-deepv" },
            { ".def", "text/plain" },
            { ".der", "application/x-x509-ca-cert" },
            { ".dif", "video/x-dv" },
            { ".dir", "application/x-director" },
            { ".dl", "video/dl" },
            { ".docm", "application/vnd.ms-word.document.macroenabled.12" },
            { ".dot", "application/msword" },
            { ".dotm", "application/vnd.ms-word.template.macroenabled.12" },
            { ".dotx", "application/vnd.openxmlformats-officedocument.wordprocessingml.template" },
            { ".dp", "application/commonground" },
            { ".drw", "application/drafting" },
            { ".dump", "application/octet-stream" },
            { ".dv", "video/x-dv" },
            { ".dvi", "application/x-dvi" },
            { ".dwf", "drawing/x-dwf (old)" },
            { ".dwg", "application/acad" },
            { ".dxf", "application/dxf" },
            { ".dxr", "application/x-director" },
            { ".el", "text/x-script.elisp" },
            { ".elc", "application/x-elc" },
            { ".env", "application/x-envoy" },
            { ".eps", "application/postscript" },
            { ".es", "application/x-esrehber" },
            { ".etx", "text/x-setext" },
            { ".evy", "application/envoy" },
            { ".f", "text/plain" },
            { ".f77", "text/x-fortran" },
            { ".f90", "text/plain" },
            { ".fdf", "application/vnd.fdf" },
            { ".fif", "application/fractals" },
            { ".fli", "video/fli" },
            { ".flo", "image/florian" },
            { ".flx", "text/vnd.fmi.flexstor" },
            { ".fmf", "video/x-atomic3d-feature" },
            { ".for", "text/x-fortran" },
            { ".fpx", "image/vnd.fpx" },
            { ".frl", "application/freeloader" },
            { ".funk", "audio/make" },
            { ".g", "text/plain" },
            { ".g3", "image/g3fax" },
            { ".gif", "image/gif" },
            { ".gl", "video/gl" },
            { ".gsd", "audio/x-gsm" },
            { ".gsm", "audio/x-gsm" },
            { ".gsp", "application/x-gsp" },
            { ".gss", "application/x-gss" },
            { ".gtar", "application/x-gtar" },
            { ".gz", "application/x-compressed" },
            { ".gzip", "application/x-gzip" },
            { ".h", "text/plain" },
            { ".hdf", "application/x-hdf" },
            { ".help", "application/x-helpfile" },
            { ".hgl", "application/vnd.hp-hpgl" },
            { ".hh", "text/plain" },
            { ".hlb", "text/x-script" },
            { ".hlp", "application/hlp" },
            { ".hpg", "application/vnd.hp-hpgl" },
            { ".hpgl", "application/vnd.hp-hpgl" },
            { ".hqx", "application/binhex" },
            { ".hta", "application/hta" },
            { ".htc", "text/x-component" },
            { ".htm", "text/html" },
            { ".htmls", "text/html" },
            { ".htt", "text/webviewhtml" },
            { ".htx", "text/html" },
            { ".ice", "x-conference/x-cooltalk" },
            { ".ico", "image/x-icon" },
            { ".idc", "text/plain" },
            { ".ief", "image/ief" },
            { ".iefs", "image/ief" },
            { ".iges", "application/iges" },
            { ".igs", "application/iges" },
            { ".ima", "application/x-ima" },
            { ".imap", "application/x-httpd-imap" },
            { ".inf", "application/inf" },
            { ".ins", "application/x-internett-signup" },
            { ".ip", "application/x-ip2" },
            { ".isu", "video/x-isvideo" },
            { ".it", "audio/it" },
            { ".iv", "application/x-inventor" },
            { ".ivr", "i-world/i-vrml" },
            { ".ivy", "application/x-livescreen" },
            { ".jam", "audio/x-jam" },
            { ".jav", "text/plain" },
            { ".java", "text/plain" },
            { ".jcm", "application/x-java-commerce" },
            { ".jfif", "image/jpeg" },
            { ".jfif-tbnl", "image/jpeg" },
            { ".jpe", "image/jpeg" },
            { ".jpeg", "image/jpeg" },
            { ".jpg", "image/jpeg" },
            { ".jps", "image/x-jps" },
            { ".js", "application/x-javascript" },
            { ".jut", "image/jutvision" },
            { ".kar", "audio/midi" },
            { ".ksh", "application/x-ksh" },
            { ".la", "audio/nspaudio" },
            { ".lam", "audio/x-liveaudio" },
            { ".latex", "application/x-latex" },
            { ".lha", "application/lha" },
            { ".lhx", "application/octet-stream" },
            { ".list", "text/plain" },
            { ".lma", "audio/nspaudio" },
            { ".log", "text/plain" },
            { ".lsp", "application/x-lisp" },
            { ".lst", "text/plain" },
            { ".lsx", "text/x-la-asf" },
            { ".ltx", "application/x-latex" },
            { ".lzh", "application/octet-stream" },
            { ".lzx", "application/lzx" },
            { ".m", "text/plain" },
            { ".m1v", "video/mpeg" },
            { ".m2a", "audio/mpeg" },
            { ".m2v", "video/mpeg" },
            { ".m3u", "audio/x-mpegurl" },
            { ".man", "application/x-troff-man" },
            { ".map", "application/x-navimap" },
            { ".mar", "text/plain" },
            { ".mbd", "application/mbedlet" },
            { ".mc$", "application/x-magic-cap-package-1.0" },
            { ".mcd", "application/mcad" },
            { ".mcf", "image/vasa" },
            { ".mcp", "application/netmc" },
            { ".me", "application/x-troff-me" },
            { ".mht", "message/rfc822" },
            { ".mhtml", "message/rfc822" },
            { ".mid", "audio/x-midi" },
            { ".midi", "audio/x-midi" },
            { ".mif", "application/x-mif" },
            { ".mime", "message/rfc822" },
            { ".mjf", "audio/x-vnd.audioexplosion.mjuicemediafile" },
            { ".mjpg", "video/x-motion-jpeg" },
            { ".mm", "application/base64" },
            { ".mme", "application/base64" },
            { ".mod", "audio/mod" },
            { ".moov", "video/quicktime" },
            { ".mov", "video/quicktime" },
            { ".movie", "video/x-sgi-movie" },
            { ".mp2", "audio/mpeg" },
            { ".mp3", "audio/mpeg3" },
            { ".mp4", "video/mp4" },
            { ".mpa", "audio/mpeg" },
            { ".mpc", "application/x-project" },
            { ".mpe", "video/mpeg" },
            { ".mpeg", "video/mpeg" },
            { ".mpg", "audio/mpeg" },
            { ".mpga", "audio/mpeg" },
            { ".mpp", "application/vnd.ms-project" },
            { ".mpt", "application/x-project" },
            { ".mpv", "application/x-project" },
            { ".mpx", "application/x-project" },
            { ".mrc", "application/marc" },
            { ".ms", "application/x-troff-ms" },
            { ".mv", "video/x-sgi-movie" },
            { ".my", "audio/make" },
            { ".mzz", "application/x-vnd.audioexplosion.mzz" },
            { ".nap", "image/naplps" },
            { ".naplps", "image/naplps" },
            { ".nc", "application/x-netcdf" },
            { ".ncm", "application/vnd.nokia.configuration-message" },
            { ".nif", "image/x-niff" },
            { ".niff", "image/x-niff" },
            { ".nix", "application/x-mix-transfer" },
            { ".nsc", "application/x-conference" },
            { ".nvd", "application/x-navidoc" },
            { ".o", "application/octet-stream" },
            { ".oda", "application/oda" },
            { ".omc", "application/x-omc" },
            { ".omcd", "application/x-omcdatamaker" },
            { ".omcr", "application/x-omcregerator" },
            { ".p", "text/x-pascal" },
            { ".p10", "application/pkcs10" },
            { ".p12", "application/pkcs-12" },
            { ".p7a", "application/x-pkcs7-signature" },
            { ".p7c", "application/pkcs7-mime" },
            { ".p7m", "application/pkcs7-mime" },
            { ".p7r", "application/x-pkcs7-certreqresp" },
            { ".p7s", "application/pkcs7-signature" },
            { ".part", "application/pro_eng" },
            { ".pas", "text/pascal" },
            { ".pbm", "image/x-portable-bitmap" },
            { ".pcl", "application/x-pcl" },
            { ".pct", "image/x-pict" },
            { ".pcx", "image/x-pcx" },
            { ".pdb", "chemical/x-pdb" },
            { ".pdf", "application/pdf" },
            { ".pfunk", "audio/make" },
            { ".pgm", "image/x-portable-graymap" },
            { ".pic", "image/pict" },
            { ".pict", "image/pict" },
            { ".pkg", "application/x-newton-compatible-pkg" },
            { ".pko", "application/vnd.ms-pki.pko" },
            { ".pl", "text/x-script.perl" },
            { ".plx", "application/x-pixclscript" },
            { ".pm", "image/x-xpixmap" },
            { ".pm4", "application/x-pagemaker" },
            { ".pm5", "application/x-pagemaker" },
            { ".png", "image/png" },
            { ".pnm", "application/x-portable-anymap" },
            { ".pot", "application/vnd.ms-powerpoint" },
            { ".potm", "application/vnd.ms-powerpoint.template.macroenabled.12" },
            { ".potx", "application/vnd.openxmlformats-officedocument.presentationml.template" },
            { ".ppa", "application/vnd.ms-powerpoint" },
            { ".ppam", "application/vnd.ms-powerpoint.addin.macroenabled.12" },
            { ".ppm", "image/x-portable-pixmap" },
            { ".pps", "application/vnd.ms-powerpoint" },
            { ".ppsm", "application/vnd.ms-powerpoint.slideshow.macroenabled.12" },
            { ".ppsx", "application/vnd.openxmlformats-officedocument.presentationml.slideshow" },
            { ".ppt", "application/vnd.ms-powerpoint" },
            { ".pptm", "application/vnd.ms-powerpoint.presentation.macroenabled.12" },
            { ".pptx", "application/vnd.openxmlformats-officedocument.presentationml.presentation" },
            { ".ppz", "application/mspowerpoint" },
            { ".pre", "application/x-freelance" },
            { ".prt", "application/pro_eng" },
            { ".ps", "application/postscript" },
            { ".psd", "application/octet-stream" },
            { ".pvu", "paleovu/x-pv" },
            { ".pwz", "application/vnd.ms-powerpoint" },
            { ".py", "text/x-script.phyton" },
            { ".pyc", "applicaiton/x-bytecode.python" },
            { ".qcp", "audio/vnd.qcelp" },
            { ".qd3", "x-world/x-3dmf" },
            { ".qd3d", "x-world/x-3dmf" },
            { ".qif", "image/x-quicktime" },
            { ".qt", "video/quicktime" },
            { ".qtc", "video/x-qtc" },
            { ".qti", "image/x-quicktime" },
            { ".qtif", "image/x-quicktime" },
            { ".ra", "audio/x-pn-realaudio" },
            { ".ram", "audio/x-pn-realaudio" },
            { ".ras", "application/x-cmu-raster" },
            { ".rast", "image/cmu-raster" },
            { ".rexx", "text/x-script.rexx" },
            { ".rf", "image/vnd.rn-realflash" },
            { ".rgb", "image/x-rgb" },
            { ".rm", "application/vnd.rn-realmedia" },
            { ".rmi", "audio/mid" },
            { ".rmm", "audio/x-pn-realaudio" },
            { ".rmp", "audio/x-pn-realaudio" },
            { ".rng", "application/ringing-tones" },
            { ".rnx", "application/vnd.rn-realplayer" },
            { ".roff", "application/x-troff" },
            { ".rp", "image/vnd.rn-realpix" },
            { ".rpm", "audio/x-pn-realaudio-plugin" },
            { ".rt", "text/richtext" },
            { ".rtf", "application/rtf" },
            { ".rtx", "application/rtf" },
            { ".rv", "video/vnd.rn-realvideo" },
            { ".s", "text/x-asm" },
            { ".s3m", "audio/s3m" },
            { ".sbk", "application/x-tbook" },
            { ".scm", "application/x-lotusscreencam" },
            { ".sdml", "text/plain" },
            { ".sdp", "application/sdp" },
            { ".sdr", "application/sounder" },
            { ".sea", "application/sea" },
            { ".set", "application/set" },
            { ".sgm", "text/sgml" },
            { ".sgml", "text/sgml" },
            { ".sh", "application/x-sh" },
            { ".shar", "application/x-shar" },
            { ".shtml", "text/html" },
            { ".sid", "audio/x-psid" },
            { ".sit", "application/x-sit" },
            { ".skd", "application/x-koan" },
            { ".skm", "application/x-koan" },
            { ".skp", "application/x-koan" },
            { ".skt", "application/x-koan" },
            { ".sl", "application/x-seelogo" },
            { ".smi", "application/smil" },
            { ".smil", "application/smil" },
            { ".snd", "audio/basic" },
            { ".sol", "application/solids" },
            { ".spc", "text/x-speech" },
            { ".spl", "application/futuresplash" },
            { ".spr", "application/x-sprite" },
            { ".sprite", "application/x-sprite" },
            { ".src", "application/x-wais-source" },
            { ".ssi", "text/x-server-parsed-html" },
            { ".ssm", "application/streamingmedia" },
            { ".sst", "application/vnd.ms-pki.certstore" },
            { ".step", "application/step" },
            { ".stl", "application/sla" },
            { ".stp", "application/step" },
            { ".sv4cpio", "application/x-sv4cpio" },
            { ".sv4crc", "application/x-sv4crc" },
            { ".svf", "image/x-dwg" },
            { ".svr", "application/x-world" },
            { ".swf", "application/x-shockwave-flash" },
            { ".t", "application/x-troff" },
            { ".talk", "text/x-speech" },
            { ".tar", "application/x-tar" },
            { ".tbk", "application/toolbook" },
            { ".tcl", "application/x-tcl" },
            { ".tcsh", "text/x-script.tcsh" },
            { ".tex", "application/x-tex" },
            { ".texi", "application/x-texinfo" },
            { ".texinfo", "application/x-texinfo" },
            { ".text", "text/plain" },
            { ".tgz", "application/x-compressed" },
            { ".tiff", "image/tiff" },
            { ".tr", "application/x-troff" },
            { ".tsi", "audio/tsp-audio" },
            { ".tsp", "application/dsptype" },
            { ".tsv", "text/tab-separated-values" },
            { ".turbot", "image/florian" },
            { ".txt", "text/plain" },
            { ".uil", "text/x-uil" },
            { ".uni", "text/uri-list" },
            { ".unis", "text/uri-list" },
            { ".unv", "application/i-deas" },
            { ".uri", "text/uri-list" },
            { ".uris", "text/uri-list" },
            { ".ustar", "application/x-ustar" },
            { ".uu", "application/octet-stream" },
            { ".uue", "text/x-uuencode" },
            { ".vcd", "application/x-cdlink" },
            { ".vcs", "text/x-vcalendar" },
            { ".vda", "application/vda" },
            { ".vdo", "video/vdo" },
            { ".vew", "application/groupwise" },
            { ".viv", "video/vivo" },
            { ".vivo", "video/vivo" },
            { ".vmd", "application/vocaltec-media-desc" },
            { ".vmf", "application/vocaltec-media-file" },
            { ".voc", "audio/voc" },
            { ".vos", "video/vosaic" },
            { ".vox", "audio/voxware" },
            { ".vqe", "audio/x-twinvq-plugin" },
            { ".vqf", "audio/x-twinvq" },
            { ".vql", "audio/x-twinvq" },
            { ".vrml", "application/x-vrml" }
        };

        private bool _ExisteFichero(string rutaArchivo)
        {
            return File.Exists(rutaArchivo);
        }

        /// <summary>
        /// Determina el tipo MIME de un archivo a partir de los "magic numbers" de ciertos tipos de archivos y devuelve el tipo MIME o null.
        /// </summary>
        /// <param name="rutaArchivo"></param>
        /// <returns></returns>
        private string _ObtenerMIMEDelContenido(string rutaArchivo)
        {
            byte[] buffer = new byte[8];
            string resultado = null;

            try
            {
                using (FileStream fs = new FileStream(rutaArchivo, FileMode.Open, FileAccess.Read))
                {
                    fs.Read(buffer, 0, buffer.Length);
                }
            }
            catch (Exception)
            {

            }

            // Comparar los primeros bytes con los "magic numbers" conocidos
            if (buffer[0] == 0x25 && buffer[1] == 0x50 && buffer[2] == 0x44 && buffer[3] == 0x46)
            {
                resultado = "application/pdf";
            }
            else if (buffer[0] == 0xFF && buffer[1] == 0xD8 && buffer[2] == 0xFF)
            {
                resultado = "image/jpeg";
            }
            else if (buffer[0] == 0x89 && buffer[1] == 0x50 && buffer[2] == 0x4E && buffer[3] == 0x47)
            {
                resultado = "image/png";
            }
            else if (buffer[0] == 0x47 && buffer[1] == 0x49 && buffer[2] == 0x46 && buffer[3] == 0x38)
            {
                resultado = "image/gif";
            }
            //else if (buffer[0] == 0x50 && buffer[1] == 0x4B && buffer[2] == 0x03 && buffer[3] == 0x04)
            //{
            //    resultado = "application/zip";
            //}
            else if (buffer[0] == 0x52 && buffer[1] == 0x61 && buffer[2] == 0x72 && buffer[3] == 0x21)
            {
                resultado = "application/x-rar-compressed";
            }
            //else if (buffer[0] == 0xD0 && buffer[1] == 0xCF && buffer[2] == 0x11 && buffer[3] == 0xE0)
            //{
            //    resultado = "application/vnd.ms-office";
            //}
            else if (buffer[0] == 0x49 && buffer[1] == 0x44 && buffer[2] == 0x33)
            {
                resultado = "audio/mpeg";
            }
            else if (buffer[4] == 0x66 && buffer[5] == 0x74 && buffer[6] == 0x79 && buffer[7] == 0x70)
            {
                resultado = "video/mp4";
            }
            return resultado;
        }

        /// <summary>
        /// Determinta el tipo MIME de un archivo a partir de su extensión que consulta en un Dicionary con el mapeo y lo devuelve, por defecto devuelve "application/octet-stream".
        /// </summary>
        /// <param name="rutaArchivo"></param>
        /// <returns></returns>
        private string _ObtenerMIMEDeLaExtension(string rutaArchivo)
        {
            string extension = Path.GetExtension(rutaArchivo);
            string mimeType = null;
            //string mimeType = "application/octet-stream"; // default MIME type

            if (extension != null && _MimeMapping.ContainsKey(extension))
            {
                mimeType = _MimeMapping[extension];
            }
            return mimeType;
        }

        /// <summary>
        /// Leer un archivo y devuelve por consola si existe el contenido del fichero o mensaje de excepción.
        /// </summary>
        /// <param name="rutaArchivo"></param>
        public void Leer(string rutaArchivo)
        {

            // Constantes
            const string EXCEPCION_FICHERO_SIN_CONTENIDO = "Existe pero no se puede leer el contenido.";

            // 0. Inicializar variables
            string resultado = null;

            // 1. Comprobar si el fichero existe
            bool existe = _ExisteFichero(rutaArchivo);

            // 2. Leerlo
            if (existe)
            {
                try
                {
                    resultado = File.ReadAllText(rutaArchivo);
                }
                catch (Exception ex) { }
            }

            // 3. Construir el resultado
            resultado = (existe) ? resultado ?? EXCEPCION_FICHERO_SIN_CONTENIDO : EXCEPCION_FICHERO_NO_EXISTE;

            // 4. Mostrar el resultado
            Console.Write("La función devuelve: " + resultado);

        }

        /// <summary>
        /// Comprueba el tamaño del archivo y devuelve en consola el tamaño o mensaje de excepción.
        /// </summary>
        /// <param name="rutaArchivo"></param>
        public void Tamaño(string rutaArchivo)
        {
            // Constantes
            const string EXCEPCION_FICHERO_SIN_TAMAÑO = "El fichero existe pero hubo un problema para obtener su tamaño.";

            // 0. Inicializar variables
            FileInfo fileInfo = null;
            string resultado = null;

            // 1. Comprobar si el fichero existe
            bool existe = _ExisteFichero(rutaArchivo);

            // 2. Obtener la información
            if (existe)
            {
                try
                {
                    fileInfo = new FileInfo(rutaArchivo);
                }
                catch (Exception ex)
                {

                }
            }

            // 3. Formatear la salida
            // fileInfo.Leng es un long
            if (fileInfo != null)
            {
                ulong miTamaño = (ulong)fileInfo.Length;
                resultado = _FormatearTamaño(miTamaño, 1);
            }

            //Tener en cuenta el tipo de operación de bus entrada/salida, de memoria

             resultado = (existe) ? (fileInfo == null) ? EXCEPCION_FICHERO_SIN_TAMAÑO : $"El tamaño del archivo \"{rutaArchivo}\" es {resultado}." : EXCEPCION_FICHERO_NO_EXISTE;

            // 4. Mostrar el resultado
            Console.WriteLine(resultado);
        }

        public string _FormatearTamaño(ulong tamaño, uint decimales = 2)
        {
            //Variables               
            string[] S_SUFIJOS = { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" }; //Esto se podría modificar, debería ser inmutable.
            uint indice = 0;
            decimal tamaño_tmp = tamaño;

            //Calulo del tamaño
            if (tamaño > 0)
            {
                indice = (uint)Math.Log(tamaño, 1024);
                tamaño_tmp /= (1L << ((int)indice * 10));
                if (Math.Round(tamaño_tmp, (int)decimales) >= 1000)
                {
                    indice += 1;
                    tamaño_tmp /= 1024;
                }
            }

            //Devuelvo
            return string.Format(CultureInfo.InvariantCulture, "{0:n" + decimales + "} {1}", tamaño_tmp, S_SUFIJOS[indice]);
        }

        //mi intento sin acabar...
        private string FormatearTamaño(ulong miTamaño)
        {
            string resultado = miTamaño.ToString() + " bytes.";
            //TODO pasar el resultado a la medida más adecuada, si son 10000 pues darlo en MB, etc... Según el número de bytes que obtenga como tamaño del archivo escribir el tamaño en la unidad más adecuada: Bytes, KBytes, MBytes, etc.
            //¿Cómo hacer el cálculo para obtener la unidad más adecuada según el tamaño?

            if (miTamaño >= 1024)
            {
                miTamaño /= 1024;
            
                if (miTamaño < 1024 )
                {
                    resultado = miTamaño.ToString() + " KBtytes.";
                }
            }

            //Si el número es inferior a 1024 escribes en Bytes
            //Si el número es superior a 1024 dividir entre 1024 -> KB
            //Si el resultado es inferior a 1024 escribir MB
            //Si el resultado es superior a 1024 dividir entre 1024 -> GB
            //y sucesivamente hasta el máximo
            //Devuelve un string con número redondeado a la unidad correspondiente con dos decimales
            return resultado;
        }

            /// <summary>
            /// Lee un archivo para conocer su MIME type y devuelve en consola el MIME o mensaje de excepción.
            /// </summary>
            /// <param name="rutaArchivo"></param>
        internal void LeerMIME(string rutaArchivo)
        {
            // Constantes
            const string EXCEPCION_FICHERO_SIN_MIME = "El fichero existe pero su tipo MIME es desconocido.";

            // 0. Inicializar variables
            string resultado = null;

            // 1. Comprobar si el fichero existe
            bool existe = _ExisteFichero(rutaArchivo);

            // 2. Obtener la información
            if (existe)
            {
                //resultado = _ObtenerMIMEDelContenido(rutaArchivo);
                //if (resultado == null)
                //{
                //    resultado = _ObtenerMIMEDeLaExtension(rutaArchivo);
                //}
            }

            // 3. Formatear la salida
            resultado = (existe) ? (resultado == null) ? EXCEPCION_FICHERO_SIN_MIME : $"El tipo de MIME del archivo \"{rutaArchivo}\" es {resultado}." : EXCEPCION_FICHERO_NO_EXISTE;

            // 4. Mostrar el resultado
            Console.WriteLine(resultado);
        }
    }
}
