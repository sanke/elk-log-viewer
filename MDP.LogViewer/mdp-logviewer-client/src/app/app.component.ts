import {Component} from '@angular/core';
import {CommonModule} from '@angular/common';
import {RouterOutlet} from '@angular/router';
import {FileUploadEvent, FileUploadModule} from "primeng/fileupload";
import {CardModule} from "primeng/card";
import {EditorModule} from "primeng/editor";
import {MessageModule} from "primeng/message";
import {MessagesModule} from "primeng/messages";
import {HttpClient} from "@angular/common/http";

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, RouterOutlet, FileUploadModule, CardModule, EditorModule, MessageModule, MessagesModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  uploadedFiles: any[] = [];

  constructor(private http: HttpClient) {
  }

  onUpload($event: FileUploadEvent) {
    for (let file of $event.files) {
      this.uploadedFiles.push(file);
    }
  }

  clearExisting() {
    console.log('clearing existing');
    this.http.delete('/api/v1/clear').subscribe((res) => {
      console.log('clearing existing done');
    });
  }
}
