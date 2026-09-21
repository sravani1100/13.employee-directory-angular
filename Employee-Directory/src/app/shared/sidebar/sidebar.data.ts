import { SidebarModel } from "./sidebar.model";

export const SIDEBAR_HEADER: SidebarModel[] = [
    { icon: 'assets/images/TezoLogo.svg', label: 'Main Logo' },
    // { icon: 'assets/images/TezoLogo_collapse.png', label: 'Mini Logo' }
    { icon: 'assets/images/handle.png', label: 'collapse logo' }
];

export const SIDEBAR_MENU: SidebarModel[] = [
    {
        label: 'ALL',
        children: [
            { label: 'Dashboard', route: '/dashboard', icon: 'assets/Horizontal-nav/Dashboard.svg' },
            { label: 'Employees', route: '/employee', icon: 'assets/Horizontal-nav/Employees.svg' }
        ]
    },
    {
        label: 'ROLE/USER MANAGEMENT',
        children: [
            { label: 'Roles', route: '/role', icon: 'assets/Horizontal-nav/Roles.svg' },
            { label: 'Access Rights', route: '/access-rights', icon: 'assets/Horizontal-nav/assign-user.svg' }
        ]
    },
    {
        label: 'CONFIGURATION',
        children: [
            {
                label: 'Project',
                route: '/project',
                icon: 'assets/Horizontal-nav/Roles.svg'
            },
            {
                label: 'Location',
                route: '/location',
                icon: 'assets/Horizontal-nav/Roles.svg'
            },
            {
                label: 'Department',
                route: '/department',
                icon: 'assets/Horizontal-nav/Roles.svg'
            }
        ]
    }
];

export const SIDEBAR_FOOTER: SidebarModel = {
    label: 'Install new update 2.3.1A',
    description: 'Allow to launch the new update for the AD application',
    actions: ['Dismiss', 'Accept']
};