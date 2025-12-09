import { Injectable } from '@angular/core';
import { TestDto } from '../interfaces/test-interfaces/testdto.interface';
import { Test } from '../interfaces/test-interfaces/test.interface';
import { TestStatusEnum } from '../../test-catalog/interface/test-status.enum';
import { TestInputType } from '../interfaces/test-interfaces/test-input-type';
import { EmployeeType } from '../enums/employee-type.enum';

@Injectable({
  providedIn: 'root'
})
export class TransformInterfacesService {

  constructor() { }

  public transformTesttoTestDto(test: Test): TestInputType {

    const formatToISOString = (dateString: string): string => {
      const date = new Date(dateString);
      return date.toISOString();
    };

    const testsdto: TestInputType = {
      id: test.id,
      name: test.name ?? '',
      description: test.description ?? '',
      start: test.start ? formatToISOString(test.start.toString()) : undefined,
      end: test.end ? formatToISOString(test.end.toString()) : undefined,
      profile: {
      id: test.profile?.id,
      name: test.profile?.name ?? '',
      url: test.profile?.url, // Enviando solo la URL del archivo
      location: test.profile?.location ?? '',
      },
      samples: {
      quantity: Number(test.samples?.quantity), // Asegurarse de que sea un número
      weight: Number(test.samples?.weight),     // Asegurarse de que sea un número
      size: Number(test.samples?.size)          // Asegurarse de que sea un número
      },
      specialInstructions: test.specialInstructions ?? '',
      enginner: {
      id: test.enginner?.id ?? 0,
      employeeNumber: test.enginner?.employeeNumber ?? '0',
      employeeType: test.enginner?.employeeType.toUpperCase() ?? EmployeeType.Engineer.toUpperCase(),
      name: test.enginner?.name ?? ''
      },
      technicians: test.technicians?.map(technician => ({
      id: technician.id ?? 0,
      employeeNumber: technician.employeeNumber,
      employeeType: technician.employeeType.toUpperCase(),
      name: technician.name
      })) ?? [],
      specifications: test.specifications?.map(specification => ({
      specificationName: specification.specificationName ?? '',
      details: specification.details
      })) ?? [],
      equipments: test.equipments?.map(equipment => ({
        id: equipment.id ?? 0,
        name: equipment.name ?? '',
        calibrationDate: equipment.calibrationDate ?? '',
        description: equipment.description ?? ''
      })) ?? [],
      status: test.status.toUpperCase() ?? TestStatusEnum.New.toUpperCase(),  
      changeStatusTest: test.changeStatusTest ?? [],   
      attachments: test.attachments?.map(attachment => ({
      id: attachment.id ?? 0,
      name: attachment.name ?? '',
      url: attachment.url ?? ''
      })) ?? [],
      idRequest: test.idRequest ?? 0,
      lastUpdatedMessage: test.lastUpdatedMessage ?? '',
      updates: test.updates ?? []
    };
    return testsdto;
  }
  

  public transFormTestDtotoTest(test: TestDto): Test {


    let testT: Test = {
        id: test.id ?? 0,
        name: test.name ?? '',
        description: test.description ?? '',
        start: test.start ? new Date(test.start) : undefined,
        end: test.end ? new Date(test.end) : undefined,
        specifications: test.specifications ?? [], // Inicializar como array vacío
        equipments: test.equipments ?? [],         // Inicializar como array vacío
        samples: test.samples,
        specialInstructions: test.specialInstructions ?? '',
        enginner: test.enginner,        
        idRequest: test.idRequest ?? 0,
        profile: test.profile,
        attachments: test.attachments ?? [],       // Inicializar como array vacío
        //lastupdated: test.lastupdated ?? '',
       // lastUpdatedBy: test.lastUpdatedBy ?? 0,
        lastUpdatedMessage: test.lastUpdatedMessage ?? '',
        //createdAt: test.createdAt ?? '',
        //createdBy: test.createdBy ?? 0,
        status: test.status ?? TestStatusEnum.New,
        changeStatusTest: test.changeStatusTest ?? []    // Inicializar como array vacío
    };
    
    testT.equipments =[];
    testT.technicians =[];
    testT.attachments =[];
    testT.specifications =[];
    testT.changeStatusTest =[];
    testT.updates =[];

    if(test.changeStatusTest&&test.changeStatusTest?.length>0)
    {
      test.changeStatusTest.forEach((tech)=>{
        testT.changeStatusTest?.push(tech);
      });
    }

    if(test.updates&&test.updates?.length>0)
    {
      test.updates.forEach((tech)=>{
        testT.updates?.push(tech);
      });
    }

    if(test.attachments&&test.attachments?.length>0)
    {
      test.attachments.forEach((tech)=>{
        testT.attachments?.push(tech);
      });
    }
    if(test.technicians&&test.technicians?.length>0)
    {
      test.technicians.forEach((tech)=>{
        testT.technicians?.push(tech);
      });
    }
    if(test.specifications&&test.specifications?.length>0)
    {
      test.specifications.forEach((tech)=>{
        testT.specifications?.push(tech);
      });
    }
    if(test.equipments&&test.equipments?.length>0)
    {
      test.equipments.forEach((tech)=>{
        testT.equipments?.push(tech);
      });
    }
    

    

    return testT;
}


}
