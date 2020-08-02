/*
 * Copyright (c) 2020, Thomas Ochsenbein
 * All rights reserved.
 * 
 * GPL-3.0-only
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, version 3.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <https://www.gnu.org/licenses/>.
 *   
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS 
 * "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT 
 * LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS 
 * FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE 
 * COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, 
 * INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, 
 * BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; 
 * LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER 
 * CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT 
 * LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN 
 * ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE 
 * POSSIBILITY OF SUCH DAMAGE.
 * 
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XMLScreenSaver
{
    public static class ImageExtension
    {


        /// <summary>
        /// Resizes the image to newSize.
        /// Maintains the aspect ratio of original image.
        /// </summary>
        public static Image Resize(this Image sourceImage, Size newSize)
        {
            if (sourceImage.Size == newSize) return sourceImage;

            int sourceWidth = sourceImage.Width;
            int sourceHeight = sourceImage.Height;
            float ratioWidth = (float)newSize.Width / (float)sourceWidth;
            float ratioHeight = (float)newSize.Height / (float)sourceHeight;
            // 768 / 1080 = 0.7111111 - requires rounding error adjustment.
            float ratio = (float)(Math.Min(ratioWidth,ratioHeight) * 1.0005);
            int newWidth = (int)(sourceWidth * ratio);
            int newHeight = (int)(sourceHeight * ratio);
            int newX = (newSize.Width - newWidth) / 2;
            int newY = (newSize.Height - newHeight) / 2;

            Image newImage = new Bitmap(newSize.Width, newSize.Height);
            using (Graphics graphicsHandle = Graphics.FromImage(newImage))
            {
                graphicsHandle.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphicsHandle.CompositingQuality = CompositingQuality.HighQuality;
                graphicsHandle.SmoothingMode = SmoothingMode.HighQuality;
                graphicsHandle.PixelOffsetMode = PixelOffsetMode.Half;
                graphicsHandle.InterpolationMode = InterpolationMode.NearestNeighbor;
                graphicsHandle.DrawImage(sourceImage, newX, newY, newWidth, newHeight);
            }
            return newImage;
        }


        /// <summary>
        /// Paints the specified color over the image with transparency.
        /// alpha value range goes from 0 = completely transparent to 255 = solid
        /// </summary>
        public static Image PaintColor(this Image sourceImage, Color paintColor, int alpha)
        {
            Image newImage = (Image)sourceImage.Clone();

            if (alpha < 0) alpha = 0;
            if (alpha > 255) alpha = 255;

            using (Graphics graphicsHandle = Graphics.FromImage(newImage))
            {
                SolidBrush semiTransparentBrush =
                    new SolidBrush(Color.FromArgb(alpha, paintColor.R, paintColor.G, paintColor.B));
                graphicsHandle.FillRectangle(semiTransparentBrush, 0, 0, newImage.Width, newImage.Height);
            }

            return newImage;
        }


    }
}
