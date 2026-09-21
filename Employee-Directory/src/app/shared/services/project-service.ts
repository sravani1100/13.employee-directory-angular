import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environments';
import { HttpClient } from '@angular/common/http';
import { ProjectModel } from '../../models/project/project.model';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class ProjectService {

  private apiUrl = `${environment.apiUrl}/Project`;

  constructor(private http: HttpClient) {}

  getProjects(): Observable<ProjectModel[]> {
    return this.http.get<ProjectModel[]>(this.apiUrl);
  }

  addProject(project: ProjectModel): Observable<ProjectModel> {
    return this.http.post<ProjectModel>(this.apiUrl, project);
  }

  updateProject(project: ProjectModel): Observable<ProjectModel> {
    return this.http.put<ProjectModel>(
      `${this.apiUrl}/${project.projectId}`,
      project
    );
  }

  deleteProject(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

}
