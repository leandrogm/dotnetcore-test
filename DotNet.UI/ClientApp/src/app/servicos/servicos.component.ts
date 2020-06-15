import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-servicos',
  templateUrl: './servicos.component.html'
})
export class ServicosComponent {
  public servicos: Servico[];
  public baseUrl: String;
  public httpClient: HttpClient;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.httpClient = http;
    this.baseUrl = baseUrl + 'api/';
  }

  ngOnInit() {
    this.loadServicos();
  }

  finalizar(idServico: number) {
    this.httpClient.patch(this.baseUrl + 'servico/' + idServico + '/status/' + Status.Finalizado, null)
      .subscribe(result => {
        this.loadServicos();
      }, error => console.error(error));
  }

  executar(idServico: number) {
    this.httpClient.patch(this.baseUrl + 'servico/' + idServico + '/status/' + Status.EmExecucao, null)
      .subscribe(result => {
        this.loadServicos();
      }, error => console.error(error));
  }

  loadServicos() {
    this.httpClient.get<Servico[]>(this.baseUrl + 'servico').subscribe(result => {
      this.servicos = result;
    }, error => console.error(error));
  }
}

interface Servico {
  id: string;
  cliente: string;
  dataAgendamento: string;
  observacao: string;
  tipoId: number;
  tipo: Tipo;
  status: Status;
  nomeStatus: string;
}

enum Status {
  Pendente = 0,
  EmExecucao = 1,
  Finalizado = 2,
}

interface Tipo {
  id: number;
  descricao: string;
  valor: number;
  servicos: [Servico];
}
