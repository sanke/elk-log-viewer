import {Component} from '@angular/core';
import {CommonModule} from '@angular/common';
import {RouterOutlet} from '@angular/router';
import {FileUploadEvent, FileUploadModule} from "primeng/fileupload";
import {CardModule} from "primeng/card";
import {EditorModule} from "primeng/editor";
import {MessageModule} from "primeng/message";
import {MessagesModule} from "primeng/messages";

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, RouterOutlet, FileUploadModule, CardModule, EditorModule, MessageModule, MessagesModule ],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  uploadedFiles: any[] = [];

  onUpload($event: FileUploadEvent) {
    for (let file of $event.files) {
      this.uploadedFiles.push(file);
    }
  }
}
