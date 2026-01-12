package com.example.businesscard
import android.os.Bundle
import androidx.activity.ComponentActivity
import androidx.activity.compose.setContent
import androidx.activity.enableEdgeToEdge
import androidx.compose.foundation.Image
import androidx.compose.foundation.layout.*
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.Surface
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.painter.Painter
import androidx.compose.ui.res.painterResource
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.text.style.TextAlign
import androidx.compose.ui.tooling.preview.Preview
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.sp
import com.example.businesscard.ui.theme.BusinessCardTheme

// Data class for list items remains the same
data class ItemData(val imageResId: Int, val text: String)

class MainActivity : ComponentActivity() {
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        enableEdgeToEdge()
        setContent {
            BusinessCardTheme {
                Surface(
                    modifier = Modifier.fillMaxSize(),
                    color = MaterialTheme.colorScheme.background
                ) {
                    BusinessCardLayout()
                }
            }
        }
    }
}

@Composable
fun ListItem(image: Painter, text: String, modifier: Modifier = Modifier) {
    Row(
        modifier = modifier
            .padding(vertical = 8.dp),
        verticalAlignment = Alignment.CenterVertically
    ) {
        Image(
            painter = image,
            contentDescription = null,
            modifier = Modifier.size(24.dp)
        )
        Spacer(modifier = Modifier.width(16.dp))
        Text(
            text = text,
            fontSize = 16.sp
        )
    }
}

@Composable
fun BusinessCardLayout() {
    // Data for the main image and text
    val mainImagePainter = painterResource(id = R.drawable.ic_launcher_foreground) // Replace with your main image
    val mainTitle = "Jennifer Doe"
    val mainSubtitle = "Android Developer Extraordinaire"

    // Data for the list items
    val contactItems = listOf(
        ItemData(imageResId = R.drawable.ic_launcher_foreground, text = "+1 (123) 456 7890"),
        ItemData(imageResId = R.drawable.ic_launcher_foreground, text = "@AndroidDev"), // Replace with a social media icon
        ItemData(imageResId = R.drawable.ic_launcher_foreground, text = "jen.doe@android.com")
    )

    Box(modifier = Modifier.fillMaxSize()) {
        Column(
            modifier = Modifier
                .fillMaxWidth()
                .align(Alignment.Center),
            horizontalAlignment = Alignment.CenterHorizontally,
            verticalArrangement = Arrangement.Center
        ) {
            Image(
                painter = mainImagePainter,
                contentDescription = "Profile picture of $mainTitle",
                modifier = Modifier
                    .size(120.dp) // Adjust size as needed
                    .padding(bottom = 8.dp)
            )
            Text(
                text = mainTitle,
                fontSize = 32.sp, // Larger font for the name
                fontWeight = FontWeight.Light,
                textAlign = TextAlign.Center
            )
            Text(
                text = mainSubtitle,
                fontSize = 16.sp,
                fontWeight = FontWeight.Bold,
                color = MaterialTheme.colorScheme.primary, // Example of using theme color
                textAlign = TextAlign.Center
            )
        }

        // Section 2: ListItems (Bottom of the screen, centered horizontally)
        Column(
            modifier = Modifier
                .align(Alignment.BottomCenter) // Align this Column to the bottom-center of the Box
                .padding(bottom = 32.dp), // Add some padding from the absolute bottom
            horizontalAlignment = Alignment.Start // Center the list items themselves
        ) {
            contactItems.forEach { itemData ->
                val imagePainter = painterResource(id = itemData.imageResId)
                ListItem(image = imagePainter, text = itemData.text)
            }
        }
    }
}

@Preview(showBackground = true, showSystemUi = true)
@Composable
fun BusinessCardLayoutPreview() {
    BusinessCardTheme {
        BusinessCardLayout()
    }
}