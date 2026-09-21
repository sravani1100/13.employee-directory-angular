export interface SidebarModel {
    label: string,
    route?: string;  
    icon?: string;         
    description?: string;    
    actions?: string[];
    children?: SidebarModel[]; 
}



