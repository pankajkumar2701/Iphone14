
import { TestBed } from '@angular/core/testing';
import { AppConfigService } from './app-config.service';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { IAppConfig } from './app-config.model';

describe('AppConfigService', () => {
    let service: AppConfigService;
    let httpMock: HttpTestingController;

    beforeEach(() => {
        TestBed.configureTestingModule({
            imports: [HttpClientTestingModule],
            providers: [AppConfigService]
        });

        service = TestBed.inject(AppConfigService);
        httpMock = TestBed.inject(HttpTestingController);
    });

    afterEach(() => {
        httpMock.verify();
    });

    it('should be created', () => {
        expect(service).toBeTruthy();
    });

    it('should load config from file', () => {
        const config: IAppConfig = {
            environment: {
                name: '',
                production: true
            },
            appInsights: {
                instrumentationKey: ''
            },
            logging: {
                console: true,
                appInsights: false
            },
            api: {
                url: ''
            },
            buildInfo: {
                number: '0',
                createdOn: ''
            },
            google: {
                clientId: ''
            },
            github: {
                clientId: ''
            },
            environments: [
                {
                    name: '',
                    apiUrl: '',
                    allow: true
                }
            ]
        };

        service.load().then(result => {
            expect(result).toBeTrue();
            expect(AppConfigService.appConfig).toEqual(config);
        });

        const req = httpMock.expectOne('/assets/config/appconfig.json');
        expect(req.request.method).toBe('GET');
        req.flush(config);
    });

});