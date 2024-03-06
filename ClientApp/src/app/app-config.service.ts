import { Injectable } from '@angular/core';
import { IAppConfig } from './app-config.model';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export class AppConfigService {
  static appConfig: IAppConfig;
  constructor(private http: HttpClient) { }

  /**
   * Loads the application configuration from a JSON file.
   * 
   * @returns Promise resolving to true if the configuration loaded successfully.
   */
  public load(): Promise<boolean> {
    return new Promise<boolean>((resolve) => {
      const jsonFile = '/assets/config/appconfig.json';
      return this.http.get(jsonFile)
        .subscribe({
          next: (response) => {
            AppConfigService.appConfig = <IAppConfig>response;
            resolve(true);
          }
        });
    });
  }
}
